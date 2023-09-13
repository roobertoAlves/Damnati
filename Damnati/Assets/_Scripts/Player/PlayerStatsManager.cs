using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    private PlayerAnimatorManager _playerAnimatorManager;
    private PlayerManager _playerManager;
    private InputHandler _inputHandler;
    
    [Header("Health Parameters")]
    [Space(15)]
    private HealthBar _healthBar;

    [Header("Stamina Parameters")]
    [Space(15)]

    [SerializeField] private float _staminaRegenerationAmount = 1;
    [SerializeField] private float _staminaRegenerationTimer = 0;
    private StaminaBar _staminaBar;

    [Header("Rage Parameters")]
    [Space(15)]
    private RageBar _rageBar;
    [SerializeField] private int _rageLevel = 10;
    [SerializeField] private float _maxRage;
    [SerializeField] private float _currentRage;
    [SerializeField] private float _rageRegenerationTimer = 0;
    [SerializeField] private float _rageRegenerationLAAmount = 3;
    [SerializeField] private float _rageRegenerationHAAmount = 5;
    protected override void Awake() 
    {  
        base.Awake();
        _playerManager = GetComponent<PlayerManager>();
        _inputHandler = FindObjectOfType<InputHandler>();

        _healthBar = FindObjectOfType<HealthBar>();
        _staminaBar = FindObjectOfType<StaminaBar>();
        _rageBar = FindObjectOfType<RageBar>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();  
    }
    private void Start() 
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        _healthBar.SetMaxhHealth(MaxHealth);
        _healthBar.SetCurrentHealth(CurrentHealth);

        MaxStamina = SetMaxStaminaFromHealthLevel();
        CurrentStamina = MaxStamina;
        _staminaBar.SetMaxStamina(MaxStamina);
        _staminaBar.SetCurrentStamina(CurrentStamina);

        _maxRage = SetMaxRageFromRageLevel();
        _currentRage = _maxRage;
        _rageBar.SetMaxRage(_maxRage);
        _rageBar.SetCurrentRage(_currentRage);
    }

    #region Set Stats from Level
    public override void HandlePoiseResetTimer()
    {
        if(PoiseResetTimer > 0)
        {
            PoiseResetTimer = PoiseResetTimer - Time.deltaTime;
        }
        else if(PoiseResetTimer <= 0 && !_playerManager.IsInteracting)
        {
            TotalPoiseDefense = ArmorPoiseBonus;
        }
    }
    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = HealthLevel * 10;
        return MaxHealth;
    }
    private float SetMaxStaminaFromHealthLevel()
    {
        MaxStamina = StaminaLevel * 10;
        return MaxStamina;
    }

    private float SetMaxRageFromRageLevel()
    {
        _maxRage = _rageLevel * 10;
        return _maxRage;
    }

    #endregion

    #region Damage Functions
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    { 
        if(_playerManager.IsInvulnerable)
        {
            return;
        }

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation);
        _healthBar.SetCurrentHealth(CurrentHealth);
        _playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }
    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    { 
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);
        _healthBar.SetCurrentHealth(CurrentHealth);
    }
    private void HandleDeath()
    {
        CurrentHealth = 0;
        _playerAnimatorManager.PlayTargetAnimation("Death_01", true);
        IsDead = true;
    }

    #endregion

    #region Combat Stamina Actions Drain
    public void StaminaDrain(int drain)
    {
        CurrentStamina = CurrentStamina - drain;
        _staminaBar.SetCurrentStamina(CurrentStamina);

        if(CurrentStamina <= -1)
        {
            CurrentStamina = 0;
        }
    }
    public void RunStaminaDrain(float drain)
    {
        CurrentStamina = CurrentStamina - drain;
        _staminaBar.SetCurrentStamina(CurrentStamina);

        if(CurrentStamina <= -1)
        {
            CurrentStamina = 0;
        }
    }
    public void RegenerateStamina()
    {
        if(_playerManager.IsInteracting)
        {
            _staminaRegenerationTimer = 0;
        }
        else
        {
            _staminaRegenerationTimer += Time.deltaTime;

            if(CurrentStamina < MaxStamina && _staminaRegenerationTimer > 1f)
            {
                CurrentStamina += _staminaRegenerationAmount * Time.deltaTime;
                _staminaBar.SetCurrentStamina(Mathf.RoundToInt(CurrentStamina));
            }
        }
    }
    public void RageDrain(int drain)
    {
        _currentRage = _currentRage - drain;
        _rageBar.SetCurrentRage(_currentRage);
    }
    public void RegenerateRage()
    {
        if(_inputHandler.IsInRage)
        {
            _rageRegenerationTimer = 0;
        }
        else
        {
            _rageRegenerationTimer += Time.deltaTime;

            if(_currentRage < _maxRage && _rageRegenerationTimer > 1f && _inputHandler.LBAttackFlag && _inputHandler.IsHitEnemy)
            {
                _currentRage += _rageRegenerationLAAmount * Time.deltaTime;
                _rageBar.SetCurrentRage(Mathf.RoundToInt(_currentRage));
            }
            else if(_currentRage < _maxRage && _rageRegenerationTimer > 1f && _inputHandler.RBAttackFlag && _inputHandler.IsHitEnemy)
            {
                _currentRage += _rageRegenerationHAAmount * Time.deltaTime;
                _rageBar.SetCurrentRage(Mathf.RoundToInt(_currentRage));
            }
        }
    }
    #endregion
}
