using FileTypeClassifier.Enums;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileTypeClassifier.ML.Objects
{
    //This class is the container class that contains the data to both predict and train the model
    public class FileData
    {
        //Add constant values for True and False since K-Mean requires floating-point values

        private const float TRUE = 1.0f;
        private const float FALSE = 0.0f;

        //Next we add the properties used for prediction training and testing
        [ColumnName("Label")]
        public float Label { get; set; }
        public float IsBinary { get; set; }
        public float IsMZHeader { get; set; }
        public float IsPKHeader { get; set; }



        //Next we create a constructor that supports both our prediction and training.
        //We optionally pass in the filename for the training to provide a label,
        //In this case, ps1,exe and doc for scripts, executables and documents, respectively




        //Implementing our helper methods


        //HasBinaryContent as the name implies, takes the raw binary data and searches for non-text characters
        //to ensure it is a binary file
        private static bool HasBinaryContent(Span<byte> fileContent) => System.Text.Encoding.UTF8.GetString(fileContent.ToArray()).Any(a => char.IsControl(a) && a != '\r' && a != '\n');

        private static bool HasHeaderBytes(Span<byte> data, string match) => System.Text.Encoding.UTF8.GetString(data) == match;
        public FileData(Span<byte> data, string? fileName=null)
        {
            //Used for training purposes only
            if(!string.IsNullOrEmpty(fileName))
            {
                  if(fileName.Contains("ps1"))
                {
                    Label = (float)FileTypes.Script;
                }
                 else if(fileName.Contains("exe"))
                {
                    Label = (float)FileTypes.Executable;
                }
                  else if(fileName.Contains("doc"))
                {

                    Label=(float)FileTypes.Document;
                }


            }
            
            

       






            //We also call helper methods to determine whether the file is binary,
            //Or Whether it starts with MZ or PK

            IsBinary= HasBinaryContent(data)? TRUE: FALSE;
            IsMZHeader=HasHeaderBytes(data.Slice(0,2),"MZ")? TRUE: FALSE;   
            IsPKHeader=HasHeaderBytes(data.Slice(0,2),"PK")?TRUE: FALSE;

           
            
            }
        //An Additional Constructor to support the hard truth setting of values

        /// <summary>
        /// Used for mapping cluster ids to results only
        /// </summary>
        /// <param name="fileType"></param>
        public FileData(FileTypes fileType)
        {
            Label = (float)fileType;

            switch (fileType)
            {
                case FileTypes.Document:
                    IsBinary = TRUE;
                    IsMZHeader = FALSE;
                    IsPKHeader = TRUE;
                    break;

                case FileTypes.Executable:
                    IsBinary = TRUE;
                    IsMZHeader = TRUE;
                    IsPKHeader = FALSE;
                    break;
                case FileTypes.Script:
                    IsBinary = FALSE;
                    IsMZHeader = FALSE;
                    IsPKHeader = FALSE;
                    break;



            }





        }

        //Lastly we override the ToString method to be used with the feature extraction

        public override string ToString()=> $"{Label}.{IsBinary},{IsMZHeader},{IsPKHeader}";
        
    }
}
