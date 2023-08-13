using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RageBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    public void SetMaxRage(float maxRage)
    {
        _slider.maxValue = maxRage;
        _slider.value = maxRage;
    }
    public void SetCurrentRage(float currentRage)
    {
        _slider.value = currentRage;
    }

}
