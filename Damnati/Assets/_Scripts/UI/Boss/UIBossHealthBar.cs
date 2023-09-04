using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBossHealthBar : MonoBehaviour
{
    private TMP_Text _bossName;
    private Slider _slider;


    #region GET & SET 
    public TMP_Text BossName { get { return _bossName; } set { _bossName = value; }}
    public Slider BossSliderHealthBar { get { return _slider; } set { _slider = value; }}
    #endregion
    private void Awake() 
    {
        _slider = GetComponentInChildren<Slider>();
        _bossName = GetComponentInChildren<TMP_Text>();  
    }

    private void Start()
    {
        SetHealthBarToInactive();
    }

    public void SetBossName(string name)
    {
        _bossName.text = name;
    }
    public void SetUIHealthBarToActive()
    {
        _slider.gameObject.SetActive(true);
    }
    public void SetHealthBarToInactive()
    {
        _slider.gameObject.SetActive(false);
    }
    public void SetBossMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }
    public void SetBossCurrentHealth(int currentHealth)
    {
        _slider.value = currentHealth;
    }
}
