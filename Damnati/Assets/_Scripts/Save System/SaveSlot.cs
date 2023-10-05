using UnityEngine;

[System.Serializable]
public class SaveSlot
{
    public string saveFileName;
    public bool isEmpty = true;
    public Sprite slotIcon; 
    public string currentLevelName = "";

    // Construtor para inicializar o SaveSlot
    public SaveSlot()
    {
        saveFileName = "";
        isEmpty = true;
        slotIcon = null;
        currentLevelName = "";
    }
}
