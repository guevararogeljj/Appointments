using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public class GetAllAppointmentsQuery : IRequest<Response<IReadOnlyList<AppointmentDto>>>
{
}
