using Microsoft.ML;
using System;
using System.IO;
using Microsoft.ML.Data;

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

    public class ChatbotInput
    {
        [LoadColumn(0)]
        public string Pregunta { get; set; }
        [LoadColumn(1)]
        public string Respuesta { get; set; }
    }

    public class ChatbotOutput
    {
        public string PredictedLabel { get; set; }
    }

    public class ChatbotTrainer
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        public ChatbotTrainer()
        {
            _mlContext = new MLContext();
        }

        public void Train(string dataPath)
        {
            if (!File.Exists(dataPath))
                throw new FileNotFoundException($"No se encontró el archivo de entrenamiento: {dataPath}");
            var data = _mlContext.Data.LoadFromTextFile<ChatbotInput>(dataPath, hasHeader: true, separatorChar: ',');
            if (data.GetRowCount() == 0)
                throw new InvalidOperationException("El archivo de entrenamiento está vacío o no tiene datos válidos.");
            
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "Label", inputColumnName: "Respuesta")
                .Append(_mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "Pregunta"))
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            
            _model = pipeline.Fit(data);
        }

        public string GetAnswer(string pregunta)
        {
            if (_model == null)
                throw new InvalidOperationException("El modelo no ha sido entrenado. Ejecuta Train primero.");
            var predEngine = _mlContext.Model.CreatePredictionEngine<ChatbotInput, ChatbotOutput>(_model);
            var prediction = predEngine.Predict(new ChatbotInput { Pregunta = pregunta });
            return prediction.PredictedLabel;
        }
    }
}
