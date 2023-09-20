using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    private EnemyManager _enemy;
    [SerializeField] private string _bossName;

    private UIBossHealthBar _bossHealthBar;
    private BossCombatStanceState _bossCombatStanceState;

    [Header("Second Phase FX")]
    [Space(15)]
    [SerializeField] private GameObject _particleFX;


    #region GET & SET
    public string BossName { get { return _bossName; } set { _bossName = value; }}
    public GameObject ParticleFX { get { return _particleFX; }}
    #endregion
    private void Awake() 
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        _enemy = GetComponent<EnemyManager>();
        _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start() 
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_enemy.EnemyStatsManager.MaxHealth);    
    }

    public void UpdateBossHealthBar(int currentHealth, int MaxHealth)
    {
        _bossHealthBar.SetBossCurrentHealth(currentHealth);

        if(currentHealth <= MaxHealth /2 && !_bossCombatStanceState.HasPhaseShifted)
        {
            _bossCombatStanceState.HasPhaseShifted = true;
            ShiftToSecondPhase();
        }

    }
    public void ShiftToSecondPhase()
    {
       _enemy.Animator.SetBool("IsInvulnerable", true);
        _enemy.Animator.SetBool("IsPhaseShifting", true);
        _enemy.EnemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        _bossCombatStanceState.HasPhaseShifted = true;
    }
}
