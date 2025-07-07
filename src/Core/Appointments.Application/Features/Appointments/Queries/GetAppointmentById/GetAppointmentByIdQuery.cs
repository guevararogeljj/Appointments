using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQuery : IRequest<AppointmentDto>
{
    public Guid Id { get; set; }
}
