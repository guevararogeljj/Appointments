using Appointments.Application.ML;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace Appointments.Application.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly ChatbotTrainer _trainer = new ChatbotTrainer();

        public async Task Train(string dataPath)
        {
            // Preprocesar el archivo CSV para evitar cortes por comas
            var tempPath = Path.GetTempFileName();
            using (var reader = new StreamReader(dataPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            using (var writer = new StreamWriter(tempPath))
            {
                csv.Read();
                csv.ReadHeader();
                writer.WriteLine("Pregunta;Respuesta");
                while (csv.Read())
                {
                    var pregunta = csv.GetField("Pregunta");
                    var respuesta = csv.GetField("Respuesta").Replace(";", " ").Replace("\n", " ").Replace("\r", " ");
                    writer.WriteLine($"{pregunta};{respuesta}");
                }
            }
            _trainer.Train(tempPath);
        }

        public async Task<string> GetAnswer(string pregunta)
        {
            return await _trainer.GetAnswer(pregunta);
        }
    }
}
