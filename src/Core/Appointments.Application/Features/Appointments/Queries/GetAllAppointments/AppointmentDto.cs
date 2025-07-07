using Appointments.Application.Features.Patients.Queries.GetAllPatients;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Description { get; set; }
    public PatientDto Patient { get; set; }
}
