using Microsoft.ML.Data;

namespace Appointments.Application.ML;

public class ChatbotInput
{
    [LoadColumn(0)]
    public string Pregunta { get; set; }
    [LoadColumn(1)]
    public string Respuesta { get; set; }
}