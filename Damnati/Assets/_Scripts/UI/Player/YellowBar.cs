using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YellowBar : MonoBehaviour
{
    private Slider _slider;
    
    private HealthBar _parentHealthBar;
    private StaminaBar _parentStaminaBar;

    
    private float _timer;


    #region GET & SET
    public Slider YellowBarSlider { get { return _slider; }}
    public float Timer { get { return _timer; } set { _timer = value; }}
    #endregion

    private void Awake() 
    {
        _slider = GetComponentInChildren<Slider>();
        _parentHealthBar = GetComponentInParent<HealthBar>();  
        _parentStaminaBar = GetComponentInParent<StaminaBar>();  
    }
    private void Update() 
    {
        if(_parentHealthBar != null)
        {
            if(_timer <= 0)
            {
                if(_slider.value > _parentHealthBar.PlayerHealthBarSlider.value)
                {
                    _slider.value = _slider.value - 0.5f;
                }
                else if(_slider.value <= _parentHealthBar.PlayerHealthBarSlider.value)
                {
                    gameObject.SetActive(false);
                }
            } 
            else
            {
                _timer = _timer - Time.deltaTime;
            }   
        }

        if(_parentStaminaBar != null)
        {
            if(_timer <= 0)
            {
                if(_slider.value > _parentStaminaBar.PlayerStaminaBarSlider.value)
                {
                    _slider.value = _slider.value - 0.5f;
                }
                else if(_slider.value <= _parentStaminaBar.PlayerStaminaBarSlider.value)
                {
                    gameObject.SetActive(false);
                }
            } 
            else
            {
                _timer = _timer - Time.deltaTime;
            }  
        }
    }
    private void OnEnable()
    {
        if(_timer <= 0)
        {
            _timer = 1f;
        }

    }
    public void SetMaxStat(int maxStat)
    {
        _slider.maxValue = maxStat;
        _slider.value = maxStat;
    }
}
