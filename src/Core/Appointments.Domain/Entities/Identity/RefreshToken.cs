namespace Appointments.Domain.Entities.Identity;

public class RefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsActive => DateTime.UtcNow <= Expires;
}
