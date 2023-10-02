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

}

   

    // SISTEMA DE SAVE COM JSON

    /*//Diretório de onde o arquivo de save ficará salvo

    private static readonly string SAVE_FOLDER = (Application.persistentDataPath + "/Saves");




    //Referencia da classe que reúne os atributos do sistema de save

    public static SaveData LocalData { get; private set; }




    public static void Save()

    {

        //Convertendo o conteúdo da classe em Json

        string json = JsonUtility.ToJson(LocalData);




        //Salvando o arquivo Json no sistema do usuário

        File.WriteAllText(SAVE_FOLDER + "/save.json", json);

    }




    //Sistema que carrega os dados salvos no arquivo Json

    public static SaveData Load()

    {

        //Verificando se o diretório onde o arquivo esta localizado existe

        //Nesse caso, se o diretório não existir ele será criado

        if (!Directory.Exists(SAVE_FOLDER))

        {




            //Criando o diretório

            Directory.CreateDirectory(SAVE_FOLDER);

        }




        //Verificando se o arquivo Json com os dados salvos já existe

        //Caso o arquivo exista, ele será convertido para o formato de classe

        //Nesse caso ele será convertido para uma classe SaveData

        //Dessa forma, podemos acessar os dados e carrega-los nos itens onde

        //Forem necessários

        if (File.Exists(SAVE_FOLDER + "/save.json"))

        {




            //Armazenando o arquivo Json em uma variável

            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.json");




            //Concertendo o arquivo Json para a classe SaveData

            //E armazenando essa classe na variável LocalData

            LocalData = JsonUtility.FromJson<SaveData>(saveString);

        }




        //Caso o arquivo Json não exista, iremos criá-lo

        else

        {

           

            //Criando uma nova instância da classe SaveData

            LocalData = new SaveData();




            //Criando um arquivo Json

            File.Create(SAVE_FOLDER + "/save.json").Dispose();




            //Salvando os dados criados

            Save();

        }




        //Retornando o arquivo com os dados carregados e armazenados dentro da variável LocalData

        return LocalData;

    }*/
