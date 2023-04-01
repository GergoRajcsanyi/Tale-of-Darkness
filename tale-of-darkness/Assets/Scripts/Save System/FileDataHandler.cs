using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool encryptData = false;
    private readonly string encryptionCodeWord = "azathoth";
    private readonly string backup = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.encryptData = useEncryption;
    }

    #region LOAD DATA
    public GameData Load(bool allowRestore = true)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    { 
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch
            {
                if (allowRestore)
                {
                    bool attemptRestored = RestoreAttempt(fullPath);
                    if (attemptRestored)
                    {
                        loadedData = Load(false);
                    }
                }
            }
        }
        return loadedData;
    }
    #endregion

    #region SAVE DATA
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        string backupFilePath = fullPath + backup;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);

            if (encryptData)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                { 
                    writer.Write(dataToStore);
                }
            }

            GameData verifiedData = Load();
            if (verifiedData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else
            {
                throw new Exception("Backup file cannot be created!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private bool RestoreAttempt(string fullPath)
    {
        bool restored = false;
        string backupFilePath = fullPath + backup;
        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                restored = true;
            }
            else 
            {
                throw new Exception("No backup file exists to restore data from");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured: " + e);
        }

        return restored;
    }

    #endregion

    #region ENCRYPTION
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
    #endregion
}
