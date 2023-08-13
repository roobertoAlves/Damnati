using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
[Header("Health Parameters")]
    [Space(15)]
    [SerializeField] private int _healthLevel = 10;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [Header("Stamina Parameters")]
    [Space(15)]
    [SerializeField] private int _staminaLevel = 10;
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;

    private bool _isDead;


    #region Get & Set

    public int HealthLevel { get { return _healthLevel; } set { _healthLevel = value; }}
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; }}
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; }}


    public int StaminaLevel { get { return _staminaLevel; } set { _staminaLevel = value; }}
    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; }}
    public float CurrentStamina { get { return _currentStamina; } set { _currentStamina = value; }}

    public bool IsDead { get { return _isDead; } set { _isDead = value; }}
    
    #endregion

}
