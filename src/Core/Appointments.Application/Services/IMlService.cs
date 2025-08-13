using Appointments.Application.ML;

namespace Appointments.Application.Services
{
    public interface IMlService
    {
        void TrainModel(string trainingDataPath, string modelPath);
        ModelOutput Predict(ModelInput input);
    }
}

