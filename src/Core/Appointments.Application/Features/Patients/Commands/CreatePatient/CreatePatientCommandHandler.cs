using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Features.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePatientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            PhoneNumber = request.PhoneNumber
        };

        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.CompleteAsync();

        return new Response<Guid> { Result = patient.Id };
    }
}
