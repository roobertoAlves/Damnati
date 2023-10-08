using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [Header("Save Slot U.I")]
    [Space(15)]
    [SerializeField] private GameObject[] _slotButtons;
    [SerializeField] private TMP_Text[] _emptyTitle;
    [SerializeField] private Button[] _trashButtons;
    [SerializeField] private GameObject _confirmDeletePanel;
    [SerializeField] private GameObject _confirmSavePanel;
    [SerializeField] private GameObject _confirmLoadPanel;
    [SerializeField] private GameObject _saveWindow;

    [Header("Save Screen Slots Title")]
    [Space(15)]
    [SerializeField] private GameObject[] _localeTitle;
    [SerializeField] private GameObject[] _hoursPlayedTitle;
    [SerializeField] private GameObject[] _lastPlayedDateTitle;

    // Arrays separados para cada tipo de informação

    [Header("Save Slot Informations")]
    [Space(15)]
    [SerializeField] private TMP_Text[] _locale;
    [SerializeField] private TMP_Text[] _hoursPlayed;
    [SerializeField] private TMP_Text[] _lastPlayedDate;
    [SerializeField] private Image[] _icon;

    private int _selectedSlot = -1;

    private string currentGameScene = ""; // Variável para rastrear a cena do jogo

    private void Start()
    {
        UpdateSlotButtons();
    }

    private void UpdateSlotButtons()
    {
        for (int i = 0; i < _slotButtons.Length; i++)
        {
            SaveData saveData = SaveSystem.LoadGame(i);

            if (saveData != null)
            {
                //Titles
                _localeTitle[i].gameObject.SetActive(true);
                _hoursPlayedTitle[i].gameObject.SetActive(true);
                _lastPlayedDateTitle[i].gameObject.SetActive(true);


                _locale[i].text = saveData.currentLevelName;
                _hoursPlayed[i].text = $"{saveData.lastHourPlayed}";
                _lastPlayedDate[i].text = $"{saveData.lastDatePlayed}";
                _icon[i].gameObject.SetActive(true);
                _emptyTitle[i].text = "";
                _trashButtons[i].interactable = true;
            }
            else
            {

                //Titles
                _localeTitle[i].gameObject.SetActive(false);
                _hoursPlayedTitle[i].gameObject.SetActive(false);
                _lastPlayedDateTitle[i].gameObject.SetActive(false);

                //Infos
                _locale[i].text = "";
                _hoursPlayed[i].text = "";
                _lastPlayedDate[i].text = "";
                _icon[i].gameObject.SetActive(false);

                _emptyTitle[i].text = "Empty Save Slot";
                _trashButtons[i].interactable = false;
            }
        }
    }

    public void SelectSlot(int slot)
    {
        _selectedSlot = slot;
    }

    public void SaveGame()
    {
        if (_selectedSlot != -1)
        {
            // Criar um objeto SaveData com as informações do jogo atual
            SaveData saveData = new SaveData();
            PlayerProfileSettings playerProfile = new PlayerProfileSettings();

            // Obter o nome da cena atual
            saveData.currentLevelName = SceneManager.GetActiveScene().name;

            // Outros dados do save
            saveData.lastHourPlayed = 10; // Exemplo: substitua pelas horas jogadas reais
            saveData.lastDatePlayed = System.DateTime.Now.ToString("dd-MM-yyyy"); // Adicionar data atual

            // Salvar o jogo no slot selecionado
            SaveSystem.SaveGame(_selectedSlot, saveData, playerProfile);

            // Atualizar a interface do usuário após o save
            UpdateSlotButtons();

            Debug.Log("Salvou");
        }
    }
    public void SaveGameInPause()
    {
        if (_selectedSlot != -1)
        {
            // Antes de salvar, defina CanOverwriteSaves como verdadeira.
            SaveSystem.CanOverwriteSaves = true;

            // Criar um objeto SaveData com as informações do jogo atual
            SaveData saveData = new SaveData();
            PlayerProfileSettings playerProfile = new PlayerProfileSettings();

            // Obter o nome da cena atual
            saveData.currentLevelName = SceneManager.GetActiveScene().name;

            // Outros dados do save
            saveData.lastHourPlayed = 10; // Exemplo: substitua pelas horas jogadas reais
            saveData.lastDatePlayed = System.DateTime.Now.ToString("dd-MM-yyyy"); // Adicionar data atual

            // Salvar o jogo no slot selecionado
            SaveSystem.SaveGame(_selectedSlot, saveData, playerProfile);

            // Restaure o valor padrão de CanOverwriteSaves.
            SaveSystem.CanOverwriteSaves = false;

            // Atualizar a interface do usuário após o save
            UpdateSlotButtons();

            Debug.Log("Salvou");
        }
    }
    public void LoadGame()
    {
        if (_selectedSlot != -1)
        {
            // Verificar se o slot está vazio
            if (!SaveSystem.SaveExists(_selectedSlot))
            {
                Debug.Log("Salvando e carregando save novo");
                // Slot vazio, criar um novo save e carregar a cena do tutorial
                SaveGame();
                GameManager.Instance.SceneLoadManager.LoadScene("Tutorial"); // Substitua pelo nome da cena do tutorial
            }
            else
            {
                // Slot tem um save, carregar a cena correspondente ao save
                SaveData saveData = SaveSystem.LoadGame(_selectedSlot);

                if (saveData != null)
                {
                    Debug.Log("Carregando um save existente");
                    // Verificar se a cena atual é uma cena de jogo (não é um menu)
                    if (IsGameScene(saveData.currentLevelName))
                    {
                        // Atualizar a variável currentGameScene
                        currentGameScene = saveData.currentLevelName;
                    }

                    GameManager.Instance.SceneLoadManager.LoadScene(saveData.currentLevelName);
                }
            }
        }
    }
    
    public void LoadGameInPause()
    {
        if(_selectedSlot != -1)
        {
             // Slot tem um save, carregar a cena correspondente ao save
            SaveData saveData = SaveSystem.LoadGame(_selectedSlot);

            if (saveData != null)
            {
                Debug.Log("Carregando um save existente");
                // Verificar se a cena atual é uma cena de jogo (não é um menu)
                if (IsGameScene(saveData.currentLevelName))
                {
                    // Atualizar a variável currentGameScene
                    currentGameScene = saveData.currentLevelName;
                }

                GameManager.Instance.SceneLoadManager.LoadScene(saveData.currentLevelName);
            }
        }
    }

    // Função para verificar se uma cena é uma cena de jogo (não é um menu)
    private bool IsGameScene(string sceneName)
    {
        return sceneName == "Tutorial";
    }
    public void DeleteGame()
    {
        if (_selectedSlot != -1)
        {
            if (SaveSystem.SaveExists(_selectedSlot))
            {
                SaveSystem.DeleteSave(_selectedSlot);
                _selectedSlot = -1;
                UpdateSlotButtons();
            }
        }
    }


    #region Open Confirm Menu's

    public void OpenConfirmDeleteSavePanel()
    {
        _confirmDeletePanel.SetActive(true);
        _saveWindow.SetActive(false);
    }

    public void ConfirmDeleteSave()
    {
        DeleteGame();
        _confirmDeletePanel.SetActive(false);
        _saveWindow.SetActive(true);
    }
    public void CancelDeleteSave()
    {
        _confirmDeletePanel.SetActive(false);
        _saveWindow.SetActive(true);
    }


    public void OpenConfirmSavePanel()
    {
        if(_selectedSlot != -1)
        {
            if(SaveSystem.SaveExists(_selectedSlot))
            {
                _saveWindow.SetActive(false);
                _confirmSavePanel.SetActive(true);
            }
            else
            {
                SaveGame();
            }
        }
    }
    public void ConfirmOverwriteSave()
    {
        SaveGameInPause();
        _confirmSavePanel.SetActive(false);
        _saveWindow.SetActive(true);
    }  
    public void CancelOverwriteSave()
    {
        _confirmSavePanel.SetActive(false);
        _saveWindow.SetActive(true);
    }


    public void OpenConfirmLoadSavePanel()
    {
        if(_selectedSlot != -1)
        {
            if(SaveSystem.SaveExists(_selectedSlot))
            {
                _saveWindow.SetActive(false);
                _confirmLoadPanel.SetActive(true);
            }
        }
    }
    public void ConfirmLoadSave()
    {
        _confirmLoadPanel.SetActive(false);
        _saveWindow.SetActive(false);
        LoadGameInPause();
    }
    public void CancelLoadSave()
    {
        _confirmLoadPanel.SetActive(false);
        _saveWindow.SetActive(true);
    }
    #endregion
}
