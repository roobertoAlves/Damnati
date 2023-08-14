using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _layouts;
    [SerializeField] private AudioSource _audioSourceButton;

    [SerializeField] private int _firstLayout;

    [Header("UI Components")]
    [SerializeField] private Slider _fxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private void Awake()
    {
        _musicVolumeSlider.onValueChanged.AddListener(delegate
        {
            GameManager.Instance.AudioManager.UpdateMusicVolume(_musicVolumeSlider.value);
           
        });
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartLayout();
        LoadSliderValue();

        float currentMusicVolume = SaveSystem.LocalData.musicVolume;
        GameManager.Instance.AudioManager.UpdateMusicVolume(currentMusicVolume);
        GameManager.Instance.AudioManager.PlayBackgroundMusic(3);

        Debug.Log(SaveSystem.LocalData.pontos);
        
    }

    private void StartLayout()
    {
        for(int i = 0; i < _layouts.Count; i++)
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

    public void StartGame()
    {
        GameManager.Instance.SceneLoadManager.LoadScene("Tutorial");
    }

    public void SaveConfig()
    {
        float volFx = _fxVolumeSlider.value;
        float volmusic = _musicVolumeSlider.value;

        SaveSystem.LocalData.fxVolume = volFx;
        SaveSystem.LocalData.musicVolume = volmusic;

        SaveSystem.Save();
    }

    private void LoadSliderValue()
    {
        _fxVolumeSlider.value = SaveSystem.LocalData.fxVolume;
        _musicVolumeSlider.value = SaveSystem.LocalData.musicVolume;
    }

    public void aumentaVolume(Slider novoSlider)
    {
        novoSlider.value += 0.1f;
        
    }

    public void diminuirVolume(Slider novoSlider)
    {
        novoSlider.value -= 0.1f;
    }
}
