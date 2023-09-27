using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAICharacterHealthBar : MonoBehaviour
{
    private Slider _slider;
    private float _timeUntilBarIsHidden;
    private AIUIYellowBar _uiYellowBar;
    private TMP_Text _damageCount;
    private int _currentDamageTaken;

    [SerializeField] private float _yellowBarTimer = 3;

    #region GET & SET

    public Slider HealthBarSlider { get { return _slider; }} 
    #endregion
    private void Awake() 
    {
        _slider = GetComponentInChildren<Slider>();    
        _uiYellowBar = GetComponentInChildren<AIUIYellowBar>();
        _damageCount = GetComponentInChildren<TMP_Text>();
    }

    private void OnDisable() 
    {
        _currentDamageTaken = 0;    
    }

    public void SetHealth(int health)
    {
        if(_uiYellowBar != null)
        {
            _uiYellowBar.gameObject.SetActive(true);
            _uiYellowBar.Timer = _yellowBarTimer;

            if(health > _slider.value)
            {
                _uiYellowBar.YellowBarSlider.value = health;
            }
        }
        _currentDamageTaken = _currentDamageTaken + Mathf.RoundToInt((_slider.value - health));
        _damageCount.text = _currentDamageTaken.ToString();
        _slider.value = health;
        _timeUntilBarIsHidden = 3;
    }
    public void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;

        if(_uiYellowBar != null)
        {
            _uiYellowBar.SetMaxStat(maxHealth);
        }
    }

    private void Update() 
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        _timeUntilBarIsHidden = _timeUntilBarIsHidden - Time.deltaTime;

        if(_slider != null)
        {
            if(_timeUntilBarIsHidden <= 0)
            {
                _timeUntilBarIsHidden = 0;
                _slider.gameObject.SetActive(false);
            }
            else
            {
                if(!_slider.gameObject.activeInHierarchy)
                {
                    _slider.gameObject.SetActive(true);
                }
            }

            if(_slider.value <= 0)
            {
                Destroy(_slider.gameObject);
            }
        }
    }
}
