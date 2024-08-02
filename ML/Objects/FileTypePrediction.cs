using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTypeClassifier.ML.Objects
{
    //This class contains the properties mapped to our prediction output
    public class FileTypePrediction
    {
        //In K-Means clustering, the PredictedClusterId Property stores the closest cluster found.
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        //The Distances array contains the distances from the data point to each of the clusters
        [ColumnName("Score")]
        public float[]? Distances;
    }
}
