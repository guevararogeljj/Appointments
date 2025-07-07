using Appointments.Domain.Entities.Identity;

namespace Appointments.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IPatientRepository Patients { get; }
    IAppointmentRepository Appointments { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IChatRoomRepository ChatRooms { get; }
    IChatMessageRepository ChatMessages { get; }
    Task<int> CompleteAsync();
}
