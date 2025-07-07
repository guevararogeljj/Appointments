using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using MediatR;

namespace Appointments.Application.Features.Patients.Queries.GetPatientById;

public class GetPatientByIdQuery : IRequest<PatientDto>
{
    public Guid Id { get; set; }
}
