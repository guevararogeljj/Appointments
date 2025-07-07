using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppointmentsDbContext _context;

    public PatientRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    public async Task<Patient> GetByIdAsync(Guid id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task AddAsync(Patient entity)
    {
        await _context.Patients.AddAsync(entity);
    }

    public async Task UpdateAsync(Patient entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Patient entity)
    {
        _context.Patients.Remove(entity);
    }
}
