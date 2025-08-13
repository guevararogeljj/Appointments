using Appointments.Application.ML;

namespace Appointments.Application.Services
{
    public class MlService : IMlService
    {
        private readonly ModelTrainer _modelTrainer;

        public MlService()
        {
            _modelTrainer = new ModelTrainer();
        }

        public void TrainModel(string trainingDataPath, string modelPath)
        {
            _modelTrainer.Train(trainingDataPath);
            _modelTrainer.SaveModel(modelPath);
        }

        public ModelOutput Predict(ModelInput input)
        {
            return _modelTrainer.Predict(input);
        }
    }
}

