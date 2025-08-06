using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PaginationX;

namespace Appointments.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentsDbContext _context;

    public AppointmentRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment> GetByIdAsync(Guid id)
    {
        return await _context.Appointments.FindAsync(id);
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync()
    {
        return await _context.Appointments.Include(a => a.Patient).ToListAsync();
    }

    public async Task<PagedResult<Appointment>> GetPagedAsync(PaginationRequest request)
    {
        var totalCount = await _context.Appointments.CountAsync();
        var items = await _context.Appointments
            .Include(a => a.Patient)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<Appointment>(items, totalCount, request.PageNumber, request.PageSize);
    }
    
    public async Task AddAsync(Appointment entity)
    {
        await _context.Appointments.AddAsync(entity);
    }

    public async Task UpdateAsync(Appointment entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Appointment entity)
    {
        _context.Appointments.Remove(entity);
    }
}
