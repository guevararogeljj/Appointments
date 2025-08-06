using Appointments.Domain.Entities;
using PaginationX;

namespace Appointments.Application.Contracts.Persistence;

public interface IAppointmentRepository
{
    Task<Appointment> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Appointment>> GetAllAsync();
    Task AddAsync(Appointment entity);
    Task UpdateAsync(Appointment entity);
    Task DeleteAsync(Appointment entity);
    Task<PagedResult<Appointment>> GetPagedAsync(PaginationRequest request);
    
}
