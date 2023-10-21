using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    private AICharacterManager _aiCharacterManager;
    [SerializeField] private string _bossName;

    private UIBossHealthBar _bossHealthBar;
    private BossCombatStanceState _bossCombatStanceState;
    private WorldEventManager _worldEventManager;

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
        _aiCharacterManager = GetComponent<AICharacterManager>();
        _worldEventManager = FindObjectOfType<WorldEventManager>();
        _bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start() 
    {
        _bossHealthBar.SetBossName(_bossName);
        _bossHealthBar.SetBossMaxHealth(_aiCharacterManager.AICharacterStatsManager.MaxHealth);    
    }

    private void Update() 
    {
        IsBossDead(_aiCharacterManager.AICharacterStatsManager.CurrentHealth);  
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
    public void IsBossDead(int currentHealth)
    {
        if(currentHealth <= 0 && _aiCharacterManager.IsDead)
        {
            _worldEventManager.BossHasBeenDefeated();
        }
    }
    public void ShiftToSecondPhase()
    {
       _aiCharacterManager.Animator.SetBool("IsInvulnerable", true);
        _aiCharacterManager.Animator.SetBool("IsPhaseShifting", true);
        _aiCharacterManager.AICharacterAnimatorManager.PlayTargetAnimation("Phase Shift", true);
        _bossCombatStanceState.HasPhaseShifted = true;
    }
}
