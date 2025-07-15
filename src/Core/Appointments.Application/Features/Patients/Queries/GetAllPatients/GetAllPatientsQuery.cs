using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Patients.Queries.GetAllPatients;

public class GetAllPatientsQuery : IRequest<Response<IReadOnlyList<PatientDto>>>
{
}
