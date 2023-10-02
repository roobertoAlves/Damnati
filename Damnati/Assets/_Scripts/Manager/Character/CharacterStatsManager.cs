using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    private CharacterManager _character;

    [Header("Name")]
    [Space(15)]
    [SerializeField] private string _characterName = "Nameless";

    [Header("Team I.D")]
    [Space(15)]
    [SerializeField] private int _teamIDNumber = 0;

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

    [Header("Armor Absorptions")]
    [Space(15)]
    [SerializeField] private float _physicalDamageAbsorptionHead;
    [SerializeField] private float _physicalDamageAbsorptionBody;
    [SerializeField] private float _physicalDamageAbsorptionLegs;
    [SerializeField] private float _physicalDamageAbsorptionHands;

    [SerializeField] private float _fireDamageAbsorptionHead;
    [SerializeField] private float _fireDamageAbsorptionBody;
    [SerializeField] private float _fireDamageAbsorptionLegs;
    [SerializeField] private float _fireDamageAbsorptionHands;

    [Header("Blocking Absorptions")]
    [Space(15)]
    [SerializeField] private float _blockingPhysicalDamageAbsorption;
    [SerializeField] private float _blockingFireDamageAbsorption;
    [SerializeField] private float _blockingStabilityRating;

    [Header("Poise")]
    [Space(15)]
    [SerializeField] private float _totalPoiseDefense; //the TOTAL poise after damage calculation
    [SerializeField] private float _offensivePoiseBonus; //The poise you GAIN during an attack with a weapon
    [SerializeField] private float _armorPoiseBonus; //The poise you GAIN from waringwhat ever you have equipped;
    [SerializeField] private float _totalPoiseResetTime = 15;
    [SerializeField] private float _poiseResetTimer;


    #region Get & Set

    public string CharacterName { get { return _characterName; } set { _characterName = value; }}
    public int TeamIDNumber { get { return _teamIDNumber; } set { _teamIDNumber = value; }}
    public int HealthLevel { get { return _healthLevel; } set { _healthLevel = value; }}
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; }}
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; }}

    public int StaminaLevel { get { return _staminaLevel; } set { _staminaLevel = value; }}
    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; }}
    public float CurrentStamina { get { return _currentStamina; } set { _currentStamina = value; }}

    public float PhysicalDamageAbsorptionBody { get { return _physicalDamageAbsorptionBody; } set { _physicalDamageAbsorptionBody = value; }}
    public float PhysicalDamageAbsorptionLegs { get { return _physicalDamageAbsorptionLegs; } set { _physicalDamageAbsorptionLegs = value; }}
    public float PhysicalDamageAbsorptionHands { get { return _physicalDamageAbsorptionHands; } set { _physicalDamageAbsorptionHands = value; }}
    public float PhysicalDamageAbsorptionHead { get { return _physicalDamageAbsorptionHead; } set { _physicalDamageAbsorptionHead = value; }}
    
    public float FireDamageAbsorptionBody { get { return _fireDamageAbsorptionBody; } set { _fireDamageAbsorptionBody = value; }}
    public float FireDamageAbsorptionLegs { get { return _fireDamageAbsorptionLegs; } set { _fireDamageAbsorptionLegs = value; }}
    public float FireDamageAbsorptionHands { get { return _fireDamageAbsorptionHands; } set { _fireDamageAbsorptionHands = value; }}
    public float FireDamageAbsorptionHead { get { return _fireDamageAbsorptionHead; } set { _fireDamageAbsorptionHead = value; }}
    
    public float BlockingPhysicalDamageAbsorption { get { return _blockingPhysicalDamageAbsorption; } set { _blockingPhysicalDamageAbsorption = value; }}
    public float BlockingFireDamageAbsorption { get { return _blockingFireDamageAbsorption; } set { _blockingFireDamageAbsorption = value; }}
    public float BlockingStabilityRating { get { return _blockingStabilityRating; } set { _blockingStabilityRating = value; }}

    public float TotalPoiseDefense { get { return _totalPoiseDefense; } set { _totalPoiseDefense = value; }}
    public float OffensivePoiseBonus { get { return _offensivePoiseBonus; } set { _offensivePoiseBonus = value; }}
    public float ArmorPoiseBonus { get { return _armorPoiseBonus; } set { _armorPoiseBonus = value; }}
    public float TotalPoiseResetTime { get { return _totalPoiseResetTime; } set { _totalPoiseResetTime = value; }}
    public float PoiseResetTimer { get { return _poiseResetTimer; } set { _poiseResetTimer = value; }}
    #endregion

    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }
    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }
    private void Start()
    {
        _totalPoiseDefense = _armorPoiseBonus;
    }

    public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
    {
        if(_character.IsDead)
        {
            return;
        }

        _character.CharacterAnimator.EraseHandIKForWeapon();

        float totalPhysicalDamageAbsorption = 1 -
        (1 - _physicalDamageAbsorptionHead / 100) *
        (1 - _physicalDamageAbsorptionBody / 100) *
        (1 - _physicalDamageAbsorptionLegs / 100) *
        (1 - _physicalDamageAbsorptionHands / 100);
        Debug.Log("TKD ABS: " + totalPhysicalDamageAbsorption);
        physicalDamage = Mathf.RoundToInt( physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
        Debug.Log("TKD TOTAL: " + physicalDamage);
        float totalFireDamageAbsorption = 1 -
        (1 - _fireDamageAbsorptionHead / 100) *
        (1 - _fireDamageAbsorptionBody / 100) *
        (1 - _fireDamageAbsorptionLegs / 100) *
        (1 - _fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));


        float finalDamage = physicalDamage + fireDamage;// + others type of damage;


        if(enemyCharacterDamagingMe.IsPerformingFullyChargedAttack)
        {
            finalDamage = finalDamage * 2;
        }
        
        CurrentHealth = Mathf.RoundToInt(CurrentHealth - finalDamage);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _character.IsDead = true;
        }
    }
    public virtual void TakeDamageAfterBlock(int physicalDamage, int fireDamage, CharacterManager enemyCharacterDamagingMe)
    {
        if(_character.IsDead)
        {
            return;
        }

        _character.CharacterAnimator.EraseHandIKForWeapon();

        float totalPhysicalDamageAbsorption = 1 -
        (1 - _physicalDamageAbsorptionHead / 100) *
        (1 - _physicalDamageAbsorptionBody / 100) *
        (1 - _physicalDamageAbsorptionLegs / 100) *
        (1 - _physicalDamageAbsorptionHands / 100);

        physicalDamage = Mathf.RoundToInt( physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
        
        float totalFireDamageAbsorption = 1 -
        (1 - _fireDamageAbsorptionHead / 100) *
        (1 - _fireDamageAbsorptionBody / 100) *
        (1 - _fireDamageAbsorptionLegs / 100) *
        (1 - _fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));


        float finalDamage = physicalDamage + fireDamage;// + others type of damage;

        if(enemyCharacterDamagingMe.IsPerformingFullyChargedAttack)
        {
            finalDamage = finalDamage * 2;
        }
        
        CurrentHealth = Mathf.RoundToInt(CurrentHealth - finalDamage);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _character.IsDead = true;
        }
    }
    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if(_character.IsDead)
        {
            return;
        }

        float totalPhysicalDamageAbsorption = 1 -
        (1 - _physicalDamageAbsorptionHead / 100) *
        (1 - _physicalDamageAbsorptionBody / 100) *
        (1 - _physicalDamageAbsorptionLegs / 100) *
        (1 - _physicalDamageAbsorptionHands / 100);

        physicalDamage = Mathf.RoundToInt( physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
        
        float totalFireDamageAbsorption = 1 -
        (1 - _fireDamageAbsorptionHead / 100) *
        (1 - _fireDamageAbsorptionBody / 100) *
        (1 - _fireDamageAbsorptionLegs / 100) *
        (1 - _fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));


        float finalDamage = physicalDamage + fireDamage;// + others type of damage;

        CurrentHealth = Mathf.RoundToInt(CurrentHealth - finalDamage);

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            _character.IsDead = true;
        }
    }
    public virtual void HandlePoiseResetTimer()
    {
        if(_poiseResetTimer > 0)
        {
            _poiseResetTimer = _poiseResetTimer - Time.deltaTime;
        }
        else
        {
            _totalPoiseDefense = _armorPoiseBonus;
        }
    }

    public virtual void DeductStamina(float staminaToDeduct)
    {   
        _currentStamina = _currentStamina - staminaToDeduct;

        if(_currentStamina <= -1)
        {
            _currentStamina = 0;
        }
    }
    public void DrainRage()
    {
        
    }
    public int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = _healthLevel * 10;
        return _maxHealth;
    }
    public float SetMaxStaminaFromStaminaLevel()
    {
        _maxStamina = _staminaLevel * 10;
        return _maxStamina;
    }

}
