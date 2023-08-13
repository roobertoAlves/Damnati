using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private AnimatorHandler _playerAnimator;
    private PlayerManager _playerManager;
    
    [Header("Health Parameters")]
    [Space(15)]
    [SerializeField] private HealthBar _healthBar;

    [Header("Stamina Parameters")]
    [Space(15)]

    [SerializeField] private float _staminaRegenerationAmount = 1;
    [SerializeField] private float _staminaRegenerationTimer = 0;
    [SerializeField] private StaminaBar _staminaBar;
    private void Awake() 
    {
        _playerAnimator = GetComponent<AnimatorHandler>();    
        _playerManager = GetComponent<PlayerManager>();
    }
    private void Start() 
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        _healthBar.SetMaxhHealth(MaxHealth);

        MaxStamina = SetMaxStaminaFromHealthLevel();
        CurrentStamina = MaxStamina;
        _staminaBar.SetMaxStamina(MaxStamina);
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

    public void TakeDamage(int damage)
    { 
        if(_playerManager.IsInvulnerable || IsDead)
        {
            return;
        }
        CurrentHealth = CurrentHealth - damage;

        _healthBar.SetCurrentHealth(CurrentHealth);

        _playerAnimator.PlayTargetAnimation("Damage_01", true);

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            _playerAnimator.PlayTargetAnimation("Death_01", true);
            IsDead = true;
        }
    }

    public void StaminaDrain(int drain)
    {
        CurrentStamina = CurrentStamina - drain;
        _staminaBar.SetCurrentStamina(CurrentStamina);
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

            if(CurrentStamina < MaxStamina && _staminaRegenerationTimer >1f )
            {
                CurrentStamina += _staminaRegenerationAmount * Time.deltaTime;
                _staminaBar.SetCurrentStamina(Mathf.RoundToInt(CurrentStamina));
            }
        }
    }
}
