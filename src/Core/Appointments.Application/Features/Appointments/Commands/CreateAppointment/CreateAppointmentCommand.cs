using MediatR;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommand : IRequest<Guid>
{
    public Guid PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Description { get; set; }
}
