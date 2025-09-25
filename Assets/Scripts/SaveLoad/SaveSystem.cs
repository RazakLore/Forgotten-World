using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to " + savePath);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from " + savePath);
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found at " + savePath);
            return null;
        }
    }

    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }
}
