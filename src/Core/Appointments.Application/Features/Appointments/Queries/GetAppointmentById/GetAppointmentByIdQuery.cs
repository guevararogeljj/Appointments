using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQuery : IRequest<Response<AppointmentDto>>
{
    public Guid Id { get; set; }
}
