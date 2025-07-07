namespace Appointments.Application.Features.Auth;

public class RegistrationResponse
{
    public string UserId { get; set; }
    public bool RegistrationSuccessful { get; set; }
    public IEnumerable<string> Errors { get; set; }
}
