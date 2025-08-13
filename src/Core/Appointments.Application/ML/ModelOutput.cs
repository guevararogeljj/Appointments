using Microsoft.ML.Data;

namespace Appointments.Application.ML
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string Categoria { get; set; }
    }
}

