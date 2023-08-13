using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.Localization.Settings;


public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SceneLoadManager _sceneLoadManager;
    [Space(5)]
    [SerializeField] private AudioManager _audioManager;

    public static GameManager Instance;    

    private void Awake()
    {
        // Verifica se já existe uma instância da classe 'GameManager'.
        if(Instance != null)
        {
            Destroy(this);      // Destrói a instância.
        }
        else
        {
            // Se não existir a instância será armazenada na variável.
            Instance = this;
        }
        // Se trocarmos de cena a classe 'GameManager' não será destruída.
        DontDestroyOnLoad(this);

        //InitializeSystem();
    }

    private void Start() 
    {
        _sceneLoadManager.LoadScene("Menu");    
    }

    // A classe 'SceneLoadManager' recebe a variável '_sceneLoadManager'.
    public SceneLoadManager SceneLoadManager => _sceneLoadManager;      // O símbolo => significa receber algo.
    public AudioManager AudioManager => _audioManager;
    
    /*
    private void InitializeSystem()
    {
        //StartCoroutine(SavedLanguage());
        SaveSystem.Load();

    }
    private IEnumerator SavedLanguage()

    {

        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)SaveSystem.LocalData.id_local];

    }
    */
}
