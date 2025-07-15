using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Patients.Queries.GetPatientById;

public class GetPatientByIdQuery : IRequest<Response<PatientDto>>
{
    public Guid Id { get; set; }
}
