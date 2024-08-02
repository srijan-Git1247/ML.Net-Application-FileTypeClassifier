The K-means trainer used in this project is based on YinYang method. All of the inputs must be of Float Type and all input must be normalized into a single feature vector.
In Clustering, we have multiple clusters representing a group of similar datapoints. With K-means, the distance between the data point and each of the clusters are the measures of which clusters the model
will return. For K-means clustering specifically, it uses the center point of each of these clusters (also called a centroid) and then calculates the distance to the data point. The smallest of these values
is the predicted cluster.



The sampledata.csv file contains 80 rows of random files comprising 30 Windows executables, 20 PowerShell scripts and 20 Word Documents. Feel free to adjust the data to fit your own observation or to 
adjust the trained model.

Here is the snippet of the data:


![image](https://github.com/user-attachments/assets/01c1c68e-e03f-43c1-bbfc-944597b0d589)











Each of these rows contains the value for the properties in FileData Class.
These correspond to Label, IsBinary, IsMZHeader, IsPKHeader respectively.


MZ and PK are considered to be magic numbers of Windows executables and modern Microsoft Office files. 
Magic numbers are unique byte strings that are found at the beginning of every file.





In addition to this, testdata.csv file contains additional data points to test the newly trained model against and evaluate. The breakdown was even with 10 Windows executables, 10 PowerShell scripts and 
10 Word Documents. 

Here is the snippet of the data.

![image](https://github.com/user-attachments/assets/095c50db-e68d-4be1-8c9f-b9a0efa7b1e8)


Run the Console Application with commandline arguments:

1. Assuming the folder of files called "TrainingData" and "TestData" exists, execute the following command. (Optional if you use the two pre-feature extracted files sampledata.csv and testdata.csv in the Data folder of this repository)
> D:\Machine Learning Projects\FileClassifier\bin\Debug\net8.0\FileTypeClassifier.exe extract "D:\Machine Learning Projects\FileClassifier\TrainingData" "D:\Machine Learning Projects\FileClassifier\TestData"
> Extracted 80 to sampledata.csv
> Extracted 30 to testdata.csv


 2. After Extracting the data, train the model by passing the newly created sampledata.csv and testdata.csv file

![Screenshot 2024-08-02 194448](https://github.com/user-attachments/assets/447ea6ca-433e-43cc-b704-802e22215454)




3. To run the model with this file, simply pass in the filename to the built application and the predicted output will show:


![Screenshot 2024-08-02 194942](https://github.com/user-attachments/assets/eed7267a-e04c-4442-88d6-07db59d33211)


















