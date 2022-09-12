using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string DataDirPath = "";
    private string DataFileName = "";
    private bool UseEncryption = false;
    private readonly string encryptionCodeWord = "kazuya";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.DataDirPath = dataDirPath;
        this.DataFileName = dataFileName;
        this.UseEncryption = useEncryption;
    }

    public GameData Load()
    {
        //use Path.Combine to account for different OS's having different path separators 
        string fullPath = Path.Combine(DataDirPath, DataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                //load the serialized data from the file
                string dataToLoad = string.Empty;
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //optionally decrypt the data
                if(UseEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to load data from file : " + fullPath + "\n" + ex);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        //use Path.Combine to account for different OS's having different path separators 
        string fullPath = Path.Combine(DataDirPath, DataFileName);
        try
        {
            //create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            //optionally encrypt the data
            if(UseEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //write sthe serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when trying to save data to file : " + fullPath + "\n" + ex);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = string.Empty;
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
