namespace Appointments.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string LastModifiedBy { get; set; } = string.Empty;
}
