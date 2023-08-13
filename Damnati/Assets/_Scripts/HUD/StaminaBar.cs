using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public void SetMaxStamina(float maxStamina)
    {
        _slider.maxValue = maxStamina;
        _slider.value = maxStamina;
    }
    public void SetCurrentStamina(float currentStamina)
    {
        _slider.value = currentStamina;
    }

}
