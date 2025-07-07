using MediatR;

namespace Appointments.Application.Features.Patients.Queries.GetAllPatients;

public class GetAllPatientsQuery : IRequest<IReadOnlyList<PatientDto>>
{
}
