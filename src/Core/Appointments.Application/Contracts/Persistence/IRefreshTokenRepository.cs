using Appointments.Domain.Entities.Identity;

namespace Appointments.Application.Contracts.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken entity);
    Task<RefreshToken> GetByTokenAsync(string token);
    void Remove(RefreshToken entity);
}
