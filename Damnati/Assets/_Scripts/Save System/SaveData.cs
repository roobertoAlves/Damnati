[System.Serializable]
public class SaveData
{
    // Player Settings Data
    public int pontos = 0;
    public Language id_local = Language.PT;
    public float fxVolume = 0.5f;
    public float musicVolume = 0.8f;

    // Save Slots Informations
    public int horasJogadas = 0;
    public string ultimaDataJogo = "";
    public string currentLevelName = "";

    // Construtor para inicializar o SaveData
    public SaveData()
    {
        pontos = 0;
        id_local = Language.PT;
        fxVolume = 0.5f;
        musicVolume = 0.8f;
        horasJogadas = 0;
        ultimaDataJogo = "";
        currentLevelName = "";
    }
}
