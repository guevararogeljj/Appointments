using Appointments.Application.Contracts.Persistence;
using Appointments.Application.Events;
using Appointments.Domain.Common;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<Response<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
        if (patient == null)
        {
            return new Response<Guid> { Error = new Error("NotFound", $"Patient with ID {request.PatientId} not found.") };
        }

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            AppointmentDate = request.AppointmentDate,
            Description = request.Description
        };

        await _unitOfWork.Appointments.AddAsync(appointment);
        await _unitOfWork.CompleteAsync();

        var appointmentCreatedEvent = new AppointmentCreatedEvent
        {
            AppointmentId = appointment.Id,
            PatientId = appointment.PatientId,
            AppointmentDate = appointment.AppointmentDate,
            Description = appointment.Description,
            PatientEmail = patient.Email
        };

        _eventPublisher.Publish(appointmentCreatedEvent);
        
        return new Response<Guid> { Result = appointment.Id };
    }
}
