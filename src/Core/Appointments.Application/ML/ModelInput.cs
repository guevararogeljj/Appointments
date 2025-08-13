using Microsoft.ML.Data;

namespace Appointments.Application.ML
{
    public class ModelInput
    {
        [LoadColumn(0)]
        public float Edad { get; set; }

        [LoadColumn(1)]
        public float Salario { get; set; }

        [LoadColumn(2)]
        public string Departamento { get; set; }

        [LoadColumn(3)]
        public string Categoria { get; set; }
    }
}

