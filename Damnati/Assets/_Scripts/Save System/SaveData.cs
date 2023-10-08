[System.Serializable]
public class SaveData
{
    // Player Settings Data

    // Save Slots Informations
    public int lastHourPlayed = 0;
    public string lastDatePlayed = "";
    public string currentLevelName = "Tutorial";

}

[System.Serializable]
public class PlayerProfileSettings
{
    public float fxVolume = 1;
    public float musicVolume = 1;
    public int points = 0;
    public Language id_local = Language.PT;
    // Outras configurações do jogador
}

