using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private PlayerManager _player;
    private bool isPaused;
    private bool _pauseMenuOpen;

    [Header("Pause")]
    [Space(15)]
    public GameObject Pause_painel;
    public GameObject Canvas;
    public string cena;

    [Header("UI Components")]
    [Space(15)]
    [SerializeField] private Slider _fxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    // Método para lidar com o pressionamento da tecla ESC
    private void HandlePauseInput()
    {
        if (_player.PlayerInput.ESCInput)
        {
            if(!_pauseMenuOpen)
            {
                isPaused = true;
                _pauseMenuOpen = true;
            }
            else if(_pauseMenuOpen)
            {
                isPaused = false;
                _pauseMenuOpen = false;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandlePauseInput();
        PauseScreen();
    }

    private void PauseScreen()
    {
        if (!isPaused)
        {
            _pauseMenuOpen = false;
            isPaused = false;
            Pause_painel.SetActive(false);
            Time.timeScale = 1f;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            // Verifique a lógica no AudioManager para pausar/despausar a música
            //GameManager.Instance.AudioManager.PauseAndUnpauseBackgroundMusic();
        }
        else
        {
            Pause_painel.SetActive(true);
            Time.timeScale = 0f;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            // Verifique a lógica no AudioManager para pausar/despausar a música
            //GameManager.Instance.AudioManager.PauseAndUnpauseBackgroundMusic();
        }
    }

    public void MenuGame()
    {
        GameManager.Instance.SceneLoadManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void Unpaused()
    {
        _pauseMenuOpen = false;
        isPaused = false;
        Time.timeScale = 1f;
        // Não defina o Time.timeScale aqui
        Pause_painel.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void SaveConfig()
    {
        float volFx = _fxVolumeSlider.value;
        float volmusic = _musicVolumeSlider.value;

        SaveSystem.LocalData.fxVolume = volFx;
        SaveSystem.LocalData.musicVolume = volmusic;

        SaveSystem.Save();
    }

    public void VolumeUp(Slider newSlide)
    {
        newSlide.value += 0.1f;
    }

    public void VolumeDown(Slider newSlide)
    {
        newSlide.value -= 0.1f;
    }
}
