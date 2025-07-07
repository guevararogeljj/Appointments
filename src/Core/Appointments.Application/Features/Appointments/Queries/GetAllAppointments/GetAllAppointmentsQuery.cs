using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public class GetAllAppointmentsQuery : IRequest<IReadOnlyList<AppointmentDto>>
{
}
