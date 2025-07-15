using Appointments.Application.Features.Auth;
using Appointments.Domain.Entities.Identity;
using Appointments.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;

namespace Appointments.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<AuthResponse>> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new Response<AuthResponse> { Error = new Error("NotFound", "User not found") };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return new Response<AuthResponse> { Error = new Error("Unauthorized", "Invalid credentials") };
        }

        var token = await GenerateJwtToken(user);
        var refreshToken = await GenerateRefreshToken(user.Id);

        return new Response<AuthResponse> { Result = new AuthResponse { Id = user.Id, UserName = user.UserName, Email = user.Email, Token = token, RefreshToken = refreshToken.Token } };
    }

    public async Task<Response<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists != null)
        {
            return new Response<RegistrationResponse> { Error = new Error("Conflict", "User with this email already exists") };
        }

        ApplicationUser user = new() { Email = request.Email, SecurityStamp = Guid.NewGuid().ToString(), UserName = request.UserName, FirstName = request.FirstName, LastName = request.LastName };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new Response<RegistrationResponse> { Error = new Error("BadRequest", string.Join(", ", result.Errors.Select(e => e.Description))) };
        }

        return new Response<RegistrationResponse> { Result = new RegistrationResponse { UserId = user.Id, RegistrationSuccessful = true, Errors = null } };
    }

    public async Task<Response<AuthResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new Response<AuthResponse> { Error = new Error("NotFound", "User not found") };
        }

        var storedRefreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken);

        if (storedRefreshToken == null || storedRefreshToken.UserId != userId || !storedRefreshToken.IsActive)
        {
            return new Response<AuthResponse> { Error = new Error("Unauthorized", "Invalid refresh token") };
        }

        _unitOfWork.RefreshTokens.Remove(storedRefreshToken);
        await _unitOfWork.CompleteAsync();

        var newAccessToken = await GenerateJwtToken(user);
        var newRefreshToken = await GenerateRefreshToken(user.Id);

        return new Response<AuthResponse> { Result = new AuthResponse { Id = user.Id, UserName = user.UserName, Email = user.Email, Token = newAccessToken, RefreshToken = newRefreshToken.Token } };
    }

    public async Task<RefreshToken> GenerateRefreshToken(string userId)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(),
            Expires = DateTime.UtcNow.AddDays(7) // Refresh token expires in 7 days
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
        await _unitOfWork.CompleteAsync();

        return refreshToken;
    }

    public async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("idUser", user.Id.ToString()),
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
            ValidateLifetime = false // We don't validate lifetime here, because the token is expired
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
