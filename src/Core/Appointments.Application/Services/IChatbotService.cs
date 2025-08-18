namespace Appointments.Application.Services
{
    public interface IChatbotService
    {
        void Train(string dataPath);
        string GetAnswer(string pregunta);
    }
}

