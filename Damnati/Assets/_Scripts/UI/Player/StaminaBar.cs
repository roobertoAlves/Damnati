using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
    [SerializeField] private float _yellowBarTimer = 3;
    private YellowBar _uiYellowBar;
    private Slider _slider;
    private int _currentDamageTaken;

    public Slider PlayerStaminaBarSlider { get { return _slider; }}
    private void Awake() 
    {
        _uiYellowBar = GetComponentInChildren<YellowBar>();
        _slider = GetComponent<Slider>();
    }
    public void SetMaxStamina(float maxStamina)
    {
        _slider.maxValue = maxStamina;
        _slider.value = maxStamina;

        if(_uiYellowBar != null)
        {
            _uiYellowBar.SetMaxStat(Mathf.RoundToInt(maxStamina));
        }
    }
    public void SetCurrentStamina(float currentStamina)
    {
        if(_uiYellowBar != null)
        {
            _uiYellowBar.gameObject.SetActive(true);
            _uiYellowBar.Timer = _yellowBarTimer;

            if(currentStamina > _slider.value)
            {
                _uiYellowBar.YellowBarSlider.value = currentStamina;
            }
        }
        _currentDamageTaken = _currentDamageTaken + Mathf.RoundToInt((_slider.value - currentStamina));
        _slider.value = currentStamina;
    }

}
