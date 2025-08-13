namespace Appointments.Application.Services
{
    public interface IMessageSender
    {
        Task SendMessageAsync(string messageBody);
    }
}

