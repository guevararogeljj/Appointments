using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Patients.Queries.GetAllPatients;

public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, Response<IReadOnlyList<PatientDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPatientsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<IReadOnlyList<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _unitOfWork.Patients.GetAllAsync();

        var dtos = patients.Select(p => new PatientDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            BirthDate = p.BirthDate,
            PhoneNumber = p.PhoneNumber
        }).ToList();

        return new Response<IReadOnlyList<PatientDto>> { Result = dtos };
    }
}
