using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    // SISTEMA DE SAVE COM BINÁRIO
    // Caminho completo para a pasta de save
    private static string SAVE_FOLDER => Path.Combine(Application.persistentDataPath, "Saves");

    // Referência da classe que reúne os atributos do sistema de save
    public static SaveData LocalData { get; private set; }

    public static void Save()
    {
        // Criando o diretório se não existir
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        // Convertendo o objeto LocalData em formato binário
        byte[] binaryData;

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (MemoryStream memoryStream = new MemoryStream())

        {
            binaryFormatter.Serialize(memoryStream, LocalData);
            binaryData = memoryStream.ToArray();
        }

        // Salvando o arquivo binário no sistema do usuário
        File.WriteAllBytes(Path.Combine(SAVE_FOLDER, "save.bin"), binaryData);
    }

    public static SaveData Load()
    {
        // Verificando se o diretório onde o arquivo está localizado existe
        if (!Directory.Exists(SAVE_FOLDER))
        {
            // Criando o diretório
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        // Verificando se o arquivo binário com os dados salvos já existe
        string saveFilePath = Path.Combine(SAVE_FOLDER, "save.bin");

        if (File.Exists(saveFilePath))
        {
            // Lendo os dados do arquivo binário
            byte[] binaryData = File.ReadAllBytes(saveFilePath);

            // Convertendo os dados binários para a classe SaveData
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream(binaryData))
            {
                LocalData = (SaveData)binaryFormatter.Deserialize(memoryStream);
            }
        }
        else
        {
            // Caso o arquivo binário não exista, criar uma nova instância da classe SaveData
            LocalData = new SaveData();

            // Salvar os dados criados
            Save();
        }

        // Retornando o objeto LocalData com os dados carregados
        return LocalData;
    }


    //JASON SAVE CODE

    /*
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
    */
}
