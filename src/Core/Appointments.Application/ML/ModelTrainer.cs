using Microsoft.ML;
using System;
using System.IO;

namespace Appointments.Application.ML
{
    public class ModelTrainer
    {
        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;
        private IDataView _dataView;

        public ModelTrainer()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public void Train(string trainingDataPath)
        {
            _dataView = _mlContext.Data.LoadFromTextFile<ModelInput>(trainingDataPath, hasHeader: true, separatorChar: ',');

            var pipeline = ProcessData();
            
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _trainedModel = trainingPipeline.Fit(_dataView);
        }

        private IEstimator<ITransformer> ProcessData()
        {
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Categoria", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Departamento", outputColumnName: "DepartamentoFeaturized"))
                .Append(_mlContext.Transforms.Concatenate("Features", "Edad", "Salario", "DepartamentoFeaturized"));

            return pipeline;
        }
        
        public void SaveModel(string modelPath)
        {
            if (_trainedModel == null)
            {
                throw new InvalidOperationException("El modelo aún no ha sido entrenado.");
            }
            _mlContext.Model.Save(_trainedModel, _dataView.Schema, modelPath);
        }

        public ModelOutput Predict(ModelInput input)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_trainedModel);
            return predictionEngine.Predict(input);
        }
    }
}

