using FileTypeClassifier.Enums;
using FileTypeClassifier.ML.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using FileTypeClassifier.ML.Objects;

namespace FileTypeClassifier.ML
{
    //Class provides prediction support in our project
    public class Predictor: BaseML
    {



        //Helper method which maps known values to the prediction clusters
        private Dictionary<uint, FileTypes> GetClusterToMap (PredictionEngineBase<FileData, FileTypePrediction> predictionEngine)
        {
            var map = new Dictionary<uint, FileTypes> ();

            var fileTypes=Enum.GetValues (typeof (FileTypes)).Cast<FileTypes>();
            foreach ( var fileType in fileTypes)
            {
                var fileData=new FileData (fileType);
                var prediciton=predictionEngine.Predict (fileData);
                map.Add(prediciton.PredictedClusterId, fileType);




            }
            return map;
        }
        public void Predict(string inputDataFile)
        {
            if (!File.Exists(ModelPath))
            {
                ////Verifying if the model exists prior to reading it
                Console.WriteLine($"Failed to find model at {ModelPath}");
                return;

            }
            if (!File.Exists(inputDataFile))
            {
                //Verifying if the input file exists before making predictions on it 
                Console.WriteLine($"Failed to find input data at {inputDataFile}");
                return;
            }

            /*Loading the model  */
            //Then we define the ITransformer Object
            ITransformer mlModel;

            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                mlModel = MlContext.Model.Load(stream, out _);
                //Stream is disposed as a result of Using statement
            }
            if (mlModel == null)
            {
                Console.WriteLine("Failed to load the model");
                return;
            }
            // Create a prediction engine
            //Pass in FileData and FileTypePrediction to the CreatePredicitionEngine Method
            var predictionEngine = MlContext.Model.CreatePredictionEngine<FileData, FileTypePrediction>(mlModel);

            //We read the file in as binary file and pass these bytes into the constructors of FileData prior to running
            //the prediction and mapping initialization

            var fileData=new FileData(File.ReadAllBytes(inputDataFile));
            var prediction= predictionEngine.Predict(fileData);
            var mapping = GetClusterToMap(predictionEngine);


            //We need to adjust the output to match the output that a k-means prediction returns, including the Euclidean distances
            Console.WriteLine(
                $"Based on input file: {inputDataFile}{Environment.NewLine}{Environment.NewLine}" +
                $"Feature Extraction: {fileData}{Environment.NewLine}{Environment.NewLine}" +
                $"The file is predicted to be a {mapping[prediction.PredictedClusterId]}{Environment.NewLine}");

            Console.WriteLine("Distances from all clusters:");

#pragma warning disable 8602
            for (uint x = 0; x < prediction.Distances.Length; x++)
            {
                Console.WriteLine($"{mapping[x + 1]}: {prediction.Distances[x]}");
            }






        }
    }
}
