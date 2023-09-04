using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{   
    [Header("Components")]
    [Space(15)]

    [SerializeField] private UIBossHealthBar _bossHealthBar;
    [SerializeField] private EnemyBossManager _boss;

    [Header("Flags")]
    [Space(15)]
    [SerializeField] private bool _bossFightIsActive;
    [SerializeField] private bool _bossHasBeenAwakened;
    [SerializeField] private bool _bossHasBeenDefeated;

    #region GET & SET
    public bool BossFightIsActive { get { return _bossFightIsActive; } set { _bossFightIsActive = value; }}
    public bool BossHasBeenAwakened { get { return _bossHasBeenAwakened; } set { _bossHasBeenAwakened = value; }}
    public bool BBossHasBeenDefeated { get { return _bossHasBeenDefeated; } set { _bossHasBeenDefeated = value; }}

    public UIBossHealthBar BossHealthBar { get { return _bossHealthBar; } set { _bossHealthBar = value; }}
    public EnemyBossManager Boss { get { return _boss; } set { _boss = value; }}
    
    #endregion

    private void Awake() 
    {
        _bossHealthBar = FindObjectOfType<UIBossHealthBar>();    
    }
    public void ActivateBossFight()
    {
        _bossFightIsActive = true;
        _bossHasBeenAwakened = true;
        _bossHealthBar.SetUIHealthBarToActive();
    }
    public void BossHasBeenDefeated()
    {
        _bossHasBeenDefeated = true;
        _bossFightIsActive = false;
    }

}
