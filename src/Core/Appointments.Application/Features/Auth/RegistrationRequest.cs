namespace Appointments.Application.Features.Auth;

public class RegistrationRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}
