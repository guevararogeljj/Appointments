using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Patients.Queries.GetAllPatients;
using MediatR;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id);

        if (appointment == null)
        {
            return null;
        }

        return new AppointmentDto
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
    }
}
