using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using Appointments.Domain.Common;
using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Response<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<AppointmentDto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id);

        if (appointment == null)
        {
            return new Response<AppointmentDto> { Error = new Error("NotFound", $"Appointment with ID {request.Id} not found.") };
        }

        var dto = new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            AppointmentDate = appointment.AppointmentDate,
            Description = appointment.Description,
            Patient = new PatientDto
            {
                Id = appointment.Patient.Id,
                FirstName = appointment.Patient.FirstName,
                LastName = appointment.Patient.LastName,
                BirthDate = appointment.Patient.BirthDate,
                PhoneNumber = appointment.Patient.PhoneNumber
            }
        };

        return new Response<AppointmentDto> { Result = dto };
    }
}
