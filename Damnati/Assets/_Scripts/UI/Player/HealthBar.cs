using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _yellowBarTimer = 3;
    private YellowBar _uiYellowBar;
    private Slider _slider;
    private int _currentDamageTaken;

    public Slider PlayerHealthBarSlider { get { return _slider; }}
    private void Awake() 
    {
        _uiYellowBar = GetComponentInChildren<YellowBar>();
        _slider = GetComponent<Slider>();
    }
    public void SetMaxhHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;

        if(_uiYellowBar != null)
        {
            _uiYellowBar.SetMaxStat(maxHealth);
        }
    }
    public void SetCurrentHealth(int currentHealth)
    {
        if(_uiYellowBar != null)
        {
            _uiYellowBar.gameObject.SetActive(true);
            _uiYellowBar.Timer = _yellowBarTimer;

            if(currentHealth > _slider.value)
            {
                _uiYellowBar.YellowBarSlider.value = currentHealth;
            }
        }
        _currentDamageTaken = _currentDamageTaken + Mathf.RoundToInt((_slider.value - currentHealth));
        _slider.value = currentHealth;
    }
}
