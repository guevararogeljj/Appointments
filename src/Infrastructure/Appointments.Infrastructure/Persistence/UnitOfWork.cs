using Appointments.Application.Contracts.Persistence;
using Appointments.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppointmentsDbContext _context;
    private PatientRepository _patientRepository;
    private AppointmentRepository _appointmentRepository;
    private RefreshTokenRepository _refreshTokenRepository;
    private ChatRoomRepository _chatRoomRepository;
    private ChatMessageRepository _chatMessageRepository;

    public UnitOfWork(AppointmentsDbContext context)
    {
        _context = context;
    }

    public IPatientRepository Patients => _patientRepository ??= new PatientRepository(_context);
    public IAppointmentRepository Appointments => _appointmentRepository ??= new AppointmentRepository(_context);
    public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository ??= new RefreshTokenRepository(_context);
    public IChatRoomRepository ChatRooms => _chatRoomRepository ??= new ChatRoomRepository(_context);
    public IChatMessageRepository ChatMessages => _chatMessageRepository ??= new ChatMessageRepository(_context);

    public async Task<int> CompleteAsync(string user)
    {
        return await _context.SaveChangesAsync(user);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
