using FileTypeClassifier.ML.Base;
using FileTypeClassifier.ML.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FileTypeClassifier.Common;




namespace FileTypeClassifier.ML
{

    //This class provides our feature extraction for the given folder of our files.
    //Once the extraction is complete, the classification and strings data is written out to the sampledata file.
    public class FeatureExtractor : BaseML
    {
        //Generalize the extraction to take the folderPath and the outputFile
        private void ExtractFolder(string folderPath,string outputFile)
        {
            if(!Directory.Exists(folderPath))
            {

                Console.WriteLine($"{folderPath} does not exist");
                return;
            }

            var files = Directory.GetFiles(folderPath);

            using (var streamWriter = new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"D:\\Machine Learning Projects\\FileTypeClassifier\\Data/{outputFile}")))
            {
                foreach (var file in files)
                {
                    var extractedData = new FileData(File.ReadAllBytes(file), file);
                    streamWriter.WriteLine(extractedData.ToString());

                }
            }
            Console.WriteLine($"Extracted {files.Length} to {outputFile}");

        }

        public void Extract(string trainingPath , string testPath)
        {
            ExtractFolder(trainingPath, Constants.SAMPLE_DATA);
            ExtractFolder(testPath, Constants.Test_DATA);

        }
    }

}
