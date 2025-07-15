namespace Appointments.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Description { get; set; }

    public Patient? Patient { get; set; }
}
