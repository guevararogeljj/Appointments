using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, Response<IReadOnlyList<AppointmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAppointmentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<IReadOnlyList<AppointmentDto>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _unitOfWork.Appointments.GetAllAsync();

        var dtos = appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            AppointmentDate = a.AppointmentDate,
            Description = a.Description,
            Patient = new PatientDto
            {
                Id = a.Patient.Id,
                FirstName = a.Patient.FirstName,
                LastName = a.Patient.LastName,
                BirthDate = a.Patient.BirthDate,
                PhoneNumber = a.Patient.PhoneNumber
            }
        }).ToList();

        return new Response<IReadOnlyList<AppointmentDto>> { Result = dtos };
    }
}
