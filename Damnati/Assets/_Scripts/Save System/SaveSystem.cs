using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string SAVE_FOLDER => Path.Combine(Application.persistentDataPath, "Saves");
    private static string SAVE_FILE_EXTENSION = ".save";

    public static SaveData LocalData { get; private set; }
    public static PlayerProfileSettings PlayerSettings { get; private set; }

    // Adicionado para controlar se os saves podem se sobrepor
    public static bool CanOverwriteSaves { get; set; } = false;

    public static bool SaveExists(int slot)
    {
        string saveFilePath = GetSaveFilePath(slot);
        return File.Exists(saveFilePath);
    }

    // Função para excluir um save em um slot específico
    public static void DeleteSave(int slot)
    {
        string saveFilePath = GetSaveFilePath(slot);
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        else
        {
            //Debug.LogWarning($"Save file for slot {slot} does not exist. No deletion performed.");
        }
    }

    public static void SaveGame(int slot, SaveData saveData, PlayerProfileSettings playerSettings)
    {
        string saveFilePath = GetSaveFilePath(slot);

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Create(saveFilePath))
        {
            // Se CanOverwriteSaves for verdadeira ou o arquivo não existir, permita a sobreposição.
            if (CanOverwriteSaves || !File.Exists(saveFilePath))
            {
                binaryFormatter.Serialize(fileStream, saveData);
                binaryFormatter.Serialize(fileStream, playerSettings);
            }
        }
    }
    public static SaveData LoadGame(int slot)
    {
        string saveFilePath = GetSaveFilePath(slot);

        if (File.Exists(saveFilePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(saveFilePath, FileMode.Open))
            {
                if (fileStream.Length > 0) // Verifique se o arquivo não está vazio
                {
                    return (SaveData)binaryFormatter.Deserialize(fileStream);
                }
                else
                {
                    //Debug.LogWarning($"Save file for slot {slot} is empty.");
                    // Se o arquivo estiver vazio, retorne um novo objeto SaveData
                    return new SaveData();
                }
            }
        }
        else
        {
            //Debug.LogWarning($"Save file for slot {slot} does not exist.");
            return null;
        }
    }


    public static PlayerProfileSettings LoadPlayerSettings()
    {
        string settingsFilePath = GetPlayerSettingsFilePath();

        if (File.Exists(settingsFilePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(settingsFilePath, FileMode.Open))
            {
                return (PlayerProfileSettings)binaryFormatter.Deserialize(fileStream);
            }
        }
        else
        {
            return new PlayerProfileSettings(); // Retorna um novo objeto vazio se não houver configurações salvas
        }
    }

    public static void SavePlayerSettings(PlayerProfileSettings playerSettings)
    {
        string settingsFilePath = GetPlayerSettingsFilePath();

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Create(settingsFilePath))
        {
            binaryFormatter.Serialize(fileStream, playerSettings);
        }
    }

    private static string GetSaveFilePath(int slot)
    {
        return Path.Combine(SAVE_FOLDER, $"save_{slot}{SAVE_FILE_EXTENSION}");
    }

    private static string GetPlayerSettingsFilePath()
    {
        return Path.Combine(SAVE_FOLDER, $"player_settings{SAVE_FILE_EXTENSION}");
    }
}
