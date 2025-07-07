using Appointments.Application.Features.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Appointments.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Appointments.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _authService.Login(request);
        if (result.Token == null)
        {
            return Unauthorized();
        }
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        var result = await _authService.Register(request);
        if (!result.RegistrationSuccessful)
        {
            return BadRequest(result.Errors);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, request.Role);
        }

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshToken(request);
        if (result.Token == null)
        {
            return Unauthorized();
        }
        return Ok(result);
    }
    [Authorize]
    [HttpGet("protegido")]
    public IActionResult Protegido()
    {
        return Ok("Acceso concedido con JWT");
    }
}
