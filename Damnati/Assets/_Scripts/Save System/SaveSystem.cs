using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem 
{
    private static readonly string SAVE_FOLDER=(Application.persistentDataPath + "/Saves");

    public static SaveData LocalData{get; private set;}

    public static void Save()
    {
        string json = JsonUtility.ToJson(LocalData);

        File.WriteAllText(SAVE_FOLDER + "/save.json",json);
    }

    public static SaveData Load()
    {
        if(!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        if(File.Exists(SAVE_FOLDER + "/save.json"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.json");

            LocalData = JsonUtility.FromJson<SaveData>(saveString);
        }
        else
        {
            LocalData = new SaveData();

            File.Create(SAVE_FOLDER + "/save.json").Dispose();

            Save();
        }

        return LocalData;
    }
}
