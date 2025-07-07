using Appointments.Domain.Entities;

namespace Appointments.Application.Contracts.Persistence;

public interface IPatientRepository
{
    Task<Patient> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Patient>> GetAllAsync();
    Task AddAsync(Patient entity);
    Task UpdateAsync(Patient entity);
    Task DeleteAsync(Patient entity);
}
