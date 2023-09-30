using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AICharacterStatsManager : CharacterStatsManager
{   
    private AICharacterManager _aiCharacterManager;
    [SerializeField] private UIAICharacterHealthBar _aiCharacterManagerHealthBar;

    [SerializeField] private bool _isBoss;
    private WaveSpawner _waveSpawner;

    #region  GET & SET
    public bool IsBoss { get { return _isBoss;}}
    #endregion
    protected override void Awake() 
    {
        base.Awake();
        _aiCharacterManager = GetComponent<AICharacterManager>();
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        _waveSpawner = FindObjectOfType<WaveSpawner>();
    }
    private void Start() 
    {
        if(!_isBoss)
        {
            _aiCharacterManagerHealthBar.SetMaxHealth(MaxHealth);
        }
    }

    #region Damage Functions
    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    { 
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);

        if(!_isBoss)
        {
            _aiCharacterManagerHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _aiCharacterManager.EnemyBossManager != null)
        {
            _aiCharacterManager.EnemyBossManager .UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }
    }

    public void BreakGuard()
    {
        _aiCharacterManager.AICharacterAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamageMe)
    {

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamageMe);

        if(!_isBoss)
        {
            _aiCharacterManagerHealthBar.SetHealth(CurrentHealth);
        }
        else if(_isBoss && _aiCharacterManager.EnemyBossManager  != null)
        {
            _aiCharacterManager.EnemyBossManager.UpdateBossHealthBar(CurrentHealth, MaxHealth);
        }

        _aiCharacterManager.AICharacterAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        _waveSpawner.EnemyDefeated();
        CurrentHealth = 0;
        _aiCharacterManager.IsDead = true;
        _aiCharacterManager.AICharacterAnimatorManager.PlayTargetAnimation("Death_01", true);
    }

    #endregion
}
