namespace Appointments.Application.Services
{
    public interface IChatbotService
    {
        Task Train(string dataPath);
        Task<string> GetAnswer(string pregunta);
    }
}

