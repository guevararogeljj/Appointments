using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities.Identity;
using Appointments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppointmentsDbContext _context;

    public RefreshTokenRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken entity)
    {
        await _context.RefreshTokens.AddAsync(entity);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);
    }

    public void Remove(RefreshToken entity)
    {
        _context.RefreshTokens.Remove(entity);
    }
}
