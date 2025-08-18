using Appointments.Application.ML;

namespace Appointments.Application.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly ChatbotTrainer _trainer = new ChatbotTrainer();

        public void Train(string dataPath)
        {
            _trainer.Train(dataPath);
        }

        public string GetAnswer(string pregunta)
        {
            return _trainer.GetAnswer(pregunta);
        }
    }
}

