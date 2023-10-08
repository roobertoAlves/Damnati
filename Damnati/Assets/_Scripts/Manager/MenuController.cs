using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _layouts;
    [SerializeField] private AudioSource _audioSourceButton;
    [SerializeField] private int _firstLayout;
    [SerializeField] private Slider _fxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    private int selectedSlot = -1;

    private void Awake()
    {
        _musicVolumeSlider.onValueChanged.AddListener(delegate
        {
            GameManager.Instance.AudioManager.UpdateMusicVolume(_musicVolumeSlider.value);
        });
    }

    private void Start()
    {
        StartLayout();
        LoadSliderValue();

        float currentMusicVolume = SaveSystem.PlayerSettings.musicVolume;
        GameManager.Instance.AudioManager.UpdateMusicVolume(currentMusicVolume);
        GameManager.Instance.AudioManager.PlayBackgroundMusic(3);

        Debug.Log(SaveSystem.PlayerSettings.points);
    }

    private void StartLayout()
    {
        for (int i = 0; i < _layouts.Count; i++)
        {
            _layouts[i].gameObject.SetActive(false);
        }

        _layouts[_firstLayout].gameObject.SetActive(true);
    }

    public void EnableLayout(int indexLayout)
    {
        _layouts[indexLayout].gameObject.SetActive(true);
    }

    public void DisableLayout(int indexLayout)
    {
        _layouts[indexLayout].gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("VC ESTÁ FORA!!!");
    }

    public void SelectSlot(int slot)
    {
        selectedSlot = slot;
    }

    public void SaveSettings()
    {
        // Carregar as configurações do jogador existentes
        PlayerProfileSettings playerProfile = SaveSystem.LoadPlayerSettings();

        float volFx = _fxVolumeSlider.value;
        float volmusic = _musicVolumeSlider.value;

        // Atualizar as configurações do jogador
        playerProfile.fxVolume = volFx;
        playerProfile.musicVolume = volmusic;

        // Salvar as configurações do jogador atualizadas
        SaveSystem.SavePlayerSettings(playerProfile);
    }

    private void LoadSliderValue()
    {
        _fxVolumeSlider.value = SaveSystem.PlayerSettings.fxVolume;
        _musicVolumeSlider.value = SaveSystem.PlayerSettings.musicVolume;
    }

    public void VolumeUp(Slider newSlider)
    {
        newSlider.value += 0.1f;
    }

    public void VolumeDown(Slider newSlider)
    {
        newSlider.value -= 0.1f;
    }
}
