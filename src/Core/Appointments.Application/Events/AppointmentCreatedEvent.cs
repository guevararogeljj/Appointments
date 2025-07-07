namespace Appointments.Application.Events;

public class AppointmentCreatedEvent
{
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Description { get; set; }
    public string PatientEmail { get; set; }
}
