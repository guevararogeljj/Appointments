using Appointments.Domain.Entities.Identity;

namespace Appointments.Application.Features.Auth;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
    Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
    Task<RefreshToken> GenerateRefreshToken(string userId);
}
