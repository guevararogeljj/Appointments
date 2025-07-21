using System.Security.Claims;
using Appointments.Application.Contracts.Persistence;
using Appointments.Domain.Common;
using Appointments.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Appointments.Application.Features.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreatePatientCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
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
        var user = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _unitOfWork.CompleteAsync(user);

        return new Response<Guid> { Result = patient.Id };
    }
}
