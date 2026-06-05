using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerX;
    public float playerY;
    public float playerZ;

    public List<string> inventoryItems = new List<string>();

    public List<string> collectedItemIDs = new List<string>();
}

public static class SaveSystem
{
    private static string SavePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "lapis_azul_autosave.json");
        }
    }

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(SavePath, json);

        Debug.Log("Autosave feito em: " + SavePath);
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("Não existe autosave.");
            return null;
        }

        string json = File.ReadAllText(SavePath);

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("Autosave carregado.");

        return data;
    }

    public static bool HasSave()
    {
        return File.Exists(SavePath);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Autosave apagado.");
        }
    }
}