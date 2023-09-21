using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    private PlayerManager _player;

    [Header("Health Parameters")]
    [Space(15)]
    private HealthBar _healthBar;

    [Header("Stamina Parameters")]
    [Space(15)]

    [SerializeField] private float _staminaRegenerationAmount = 1;
    [SerializeField] private float _staminaRegenerationAmountWhilstBlocking = 0.1f;
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

    #region GET & SET
    public StaminaBar StaminaBar { get { return _staminaBar; }}
    #endregion
    protected override void Awake() 
    {  
        base.Awake();
        _player = GetComponent<PlayerManager>();
        _healthBar = FindObjectOfType<HealthBar>();
        _staminaBar = FindObjectOfType<StaminaBar>();
        _rageBar = FindObjectOfType<RageBar>();
    }
    private void Start() 
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        _healthBar.SetMaxhHealth(MaxHealth);
        _healthBar.SetCurrentHealth(CurrentHealth);

        MaxStamina = SetMaxStaminaFromStaminaLevel();
        CurrentStamina = MaxStamina;
        _staminaBar.SetMaxStamina(MaxStamina);
        _staminaBar.SetCurrentStamina(CurrentStamina);

        _maxRage = SetMaxRageFromRageLevel();
        _currentRage = _maxRage;
        _rageBar.SetMaxRage(_maxRage);
        _rageBar.SetCurrentRage(_currentRage);
    }

    #region Damage Functions
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamageMe)
    { 
        if(_player.IsInvulnerable)
        {
            return;
        }

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamageMe);
        _healthBar.SetCurrentHealth(CurrentHealth);
        _player.PlayerAnimator.PlayTargetAnimation(damageAnimation, true);

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
        _player.IsDead = true;
        _player.PlayerAnimator.PlayTargetAnimation("Death_01", true);
    }

    #endregion

    public float SetMaxRageFromRageLevel()
    {
        _maxRage = _rageLevel * 10;
        return _maxRage;
    }

    #region Combat Stamina Actions Drain
    public override void DeductStamina(float staminaToDeduct)
    {
        base.DeductStamina(staminaToDeduct);
        _staminaBar.SetCurrentStamina(Mathf.RoundToInt(CurrentStamina));
    }
    public void RegenerateStamina()
    {
        if(_player.IsInteracting)
        {
            _staminaRegenerationTimer = 0;
        }
        else
        {
            _staminaRegenerationTimer += Time.deltaTime;

            if(CurrentStamina < MaxStamina && _staminaRegenerationTimer > 1f)
            {
                if(_player.IsBlocking)
                {
                    CurrentStamina += _staminaRegenerationAmountWhilstBlocking * Time.deltaTime;
                    _staminaBar.SetCurrentStamina(Mathf.RoundToInt(CurrentStamina));
                }
                else
                {
                    CurrentStamina += _staminaRegenerationAmount * Time.deltaTime;
                    _staminaBar.SetCurrentStamina(Mathf.RoundToInt(CurrentStamina)); 
                }
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
        if(_player.PlayerInput.IsInRage)
        {
            _rageRegenerationTimer = 0;
        }
        else
        {
            _rageRegenerationTimer += Time.deltaTime;

            if(_currentRage < _maxRage && _rageRegenerationTimer > 1f && _player.PlayerInput.LBInput && _player.PlayerInput.IsHitEnemy)
            {
                _currentRage += _rageRegenerationLAAmount * Time.deltaTime;
                _rageBar.SetCurrentRage(Mathf.RoundToInt(_currentRage));
            }
            else if(_currentRage < _maxRage && _rageRegenerationTimer > 1f && _player.PlayerInput.RBInput && _player.PlayerInput.IsHitEnemy)
            {
                _currentRage += _rageRegenerationHAAmount * Time.deltaTime;
                _rageBar.SetCurrentRage(Mathf.RoundToInt(_currentRage));
            }
        }
    }
    
    #endregion
}
