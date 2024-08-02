using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileTypeClassifier.ML.Base;
using FileTypeClassifier.ML.Objects;
using FileTypeClassifier.Common;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace FileTypeClassifier.ML
{
    public class Trainer:BaseML
    {
        //Added GetDataView helper method which builds the IDataView object from the columns previously defined in the FileData class:

        private IDataView GetDataView(string fileName)
        {
            return MlContext.Data.LoadFromTextFile(path: fileName, columns: new[]
            {
                new TextLoader.Column(nameof(FileData.Label), DataKind.Single,0),
                new TextLoader.Column(nameof(FileData.IsBinary), DataKind.Single,1),
                new TextLoader.Column(nameof(FileData.IsMZHeader), DataKind.Single,2),
                new TextLoader.Column(nameof(FileData.IsPKHeader), DataKind.Single,3)






            }, 
            hasHeader :false,
            separatorChar:','







            );
        }

        public void Train(string trainingFileName, string testingFileName)
        {
            // Check if training data exists
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find the training data file {trainingFileName}");
                return;
            }
            // Check if testing data exists

            if (!File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }





            //Loads Text file into an IDataViewObject
            var trainingDataView = GetDataView(trainingFileName);

            //We then build the data process pipeline, transforming the columns into a single Features Column




            var dataProcessPipeLine = MlContext.Transforms.Concatenate(FEATURES, nameof(FileData.IsBinary), nameof(FileData.IsMZHeader), nameof(FileData.IsPKHeader));


            //We can now create the K-Means trainer with a cluster size of 3 and create the model

            var trainer = MlContext.Clustering.Trainers.KMeans(featureColumnName: FEATURES, numberOfClusters: 3);

            //Complete our pipeline by appending the trainer we instantiated
            var trainingPipeLine = dataProcessPipeLine.Append(trainer);

            //Train the model with the data set created Earlier
            var trainedModel = trainingPipeLine.Fit(trainingDataView);

            //Save created model to the filename specified matching training set's schema
            MlContext.Model.Save(trainedModel, trainingDataView.Schema,ModelPath);


            //Now we evaluate the model we just trained using the testing dataset

            var testingDataView=GetDataView(testingFileName);

            IDataView testDataView = trainedModel.Transform(testingDataView);

            ClusteringMetrics modelMetrics = MlContext.Clustering.Evaluate(
                data: testDataView,
                labelColumnName: "Label",
                scoreColumnName: "Score",
                featureColumnName: FEATURES

                );


            // we output all of the classification metrics




            Console.WriteLine($"Average Distance: {modelMetrics.AverageDistance}");
            Console.WriteLine($"Davies Bould Index: {modelMetrics.DaviesBouldinIndex}");
            Console.WriteLine($"Normalized Mutual Information: {modelMetrics.NormalizedMutualInformation}");

        }






    }
}
