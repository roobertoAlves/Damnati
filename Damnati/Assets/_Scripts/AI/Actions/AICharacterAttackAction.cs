using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/A.I Actions/Attack Actions")]
public class AICharacterAttackAction : AICharacterActions
{
    [SerializeField] private bool _canCombo;
    [SerializeField] private AICharacterAttackAction _comboAction;
    [SerializeField] private int _attackScore = 3;
    [SerializeField] private float _recoveryTime = 2;

    [SerializeField] private float _maximumAttackAngle = 35;
    [SerializeField] private float _minimumAttackAngle = -35;

    [SerializeField] private float _minimumDistanceNeededToAttack = 0;
    [SerializeField] private float _maximumDistanceNeededToAttack = 3;


    #region GET & SET

    public bool CanCombo { get { return _canCombo; } set { _canCombo = value; }}
    public AICharacterAttackAction ComboAction { get { return _comboAction; } set { _comboAction = value; }}

    public int AttackScore { get { return _attackScore; } set { _attackScore = value; }}
    public float RecoveryTime { get { return _recoveryTime; } set { _recoveryTime = value; }}
    
    public float MaximumAttackAngle { get { return _maximumAttackAngle; } set { _maximumAttackAngle = value; }}
    public float MinimumAttackAngle { get { return _minimumAttackAngle; } set { _minimumAttackAngle = value; }} 
    
    public float MinimumDistanceNeededToAttack { get { return _minimumDistanceNeededToAttack; } set { _minimumDistanceNeededToAttack = value; }}
    public float MaximumDistanceNeededToAttack { get { return _maximumDistanceNeededToAttack; } set { _maximumDistanceNeededToAttack = value; }}
    
    #endregion
}
