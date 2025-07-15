using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Patients.Queries.GetPatientById;

public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Response<PatientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPatientByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.Id);

        if (patient == null)
        {
            return new Response<PatientDto> { Error = new Error("NotFound", $"Patient with ID {request.Id} not found.") };
        }

        var dto = new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            PhoneNumber = patient.PhoneNumber
        };

        return new Response<PatientDto> { Result = dto };
    }
}
