using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEnemyHealthBar : MonoBehaviour
{
    private Slider _slider;
    float _timeUntilBarIsHidden;

    private void Awake() 
    {
        _slider = GetComponentInChildren<Slider>();    
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
        _timeUntilBarIsHidden = 3;
    }
    public void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }

    private void Update() 
    {
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
