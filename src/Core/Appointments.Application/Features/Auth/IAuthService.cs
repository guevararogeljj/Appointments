using Appointments.Domain.Common;
using Appointments.Domain.Entities.Identity;

namespace Appointments.Application.Features.Auth;

public interface IAuthService
{
    Task<Response<AuthResponse>> Login(AuthRequest request);
    Task<Response<RegistrationResponse>> Register(RegistrationRequest request);
    Task<Response<AuthResponse>> RefreshToken(RefreshTokenRequest request);
    Task<RefreshToken> GenerateRefreshToken(string userId);
}
