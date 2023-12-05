using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SceneLoadManager _sceneLoadManager;
    [Space(5)]
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private InputHandler _inputHandler;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
        InitializeSystem();
    }

    private void Start()
    {
        _sceneLoadManager.LoadScene("Menu");
    }

    public SceneLoadManager SceneLoadManager => _sceneLoadManager;

    public AudioManager AudioManager => _audioManager;
    public InputHandler InputHandler => _inputHandler;


    private void InitializeSystem()
    {
        SaveSystem.LoadPlayerSettings();
        StartCoroutine(SavedLanguage());
    }

    private IEnumerator SavedLanguage()
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)SaveSystem.PlayerSettings.id_local];
    }
}
