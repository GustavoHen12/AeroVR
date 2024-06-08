using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class JSONDataManager : MonoBehaviour
{
    private string dataFolderPath;

    public JSONDataManager() {
        Awake();
    }

    public void Awake() {
        dataFolderPath = Application.persistentDataPath + "/Data/";
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath);
        }
    }

    // Save data to JSON file
    public void SaveData<T>(T data, string fileName) {
        string filePath = Path.Combine(dataFolderPath, fileName + ".json");
        Debug.Log("Saving data to: " + filePath);
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log("Data: " + jsonData);
        File.WriteAllText(filePath, jsonData);
    }

    // Load data from JSON file
    public T LoadData<T>(string fileName) {
        string filePath = Path.Combine(dataFolderPath, fileName + ".json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogError("No data file found at: " + filePath);
            return default(T);
        }
    }

    public void ClearData(string fileName)
    {
        string filePath = Path.Combine(dataFolderPath, fileName + ".json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
