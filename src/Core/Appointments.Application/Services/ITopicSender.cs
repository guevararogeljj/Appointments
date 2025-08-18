using System.Threading.Tasks;

namespace Appointments.Application.Services
{
    public interface ITopicSender
    {
        Task SendMessageAsync(string messageBody);
    }
}

