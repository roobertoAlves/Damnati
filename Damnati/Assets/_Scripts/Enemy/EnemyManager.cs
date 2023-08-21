using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : CharacterManager
{
    [Header("A.I Scripts Components")]
    [Space(15)]
    private EnemyLocomotion _enemyController;
    private EnemyAnimatorController _enemyAnimation;
    private EnemyStats _enemyStats;


    [SerializeField] private States _currentState;
    [SerializeField] private CharacterStats _currentTarget;


    private bool _isPerformingAction;
    private bool _isInteracting;

    [Header("A.I Components")]
    [Space(15)]
    private Rigidbody _enemyRb;

    [Header("A.I NavMesh Agent")]
    [Space(15)]
    private NavMeshAgent _navMeshAgent;

    [Header("A.I Settings")]
    [Space(15)]
    [SerializeField] private float _detectionRadius = 20;
    [SerializeField] private float _minimumDetectionAngle = -50;
    [SerializeField] private float _maximumDetectionAngle = 50;

    [SerializeField] private float _currentRecoveryTime = 0;

    [Header("A.I Movement")]
    [Space(15)]
    [SerializeField] private float _rotationSpeed = 15;

    [Header("A.I Attack Values")]
    [Space(15)]
    [SerializeField] private float _maximumAttackRange = 1.5f;

    #region Get & Set

    public float DetectionRadius { get { return _detectionRadius; } set { _detectionRadius = value; }}

    public float MinimumDetectionAngle {get { return _minimumDetectionAngle; } set { _minimumDetectionAngle = value; }}
    public float MaximumDetectionAngle { get { return _maximumDetectionAngle; } set { _maximumDetectionAngle = value; }}
    
    public float CurrentRecoveryTime { get { return _currentRecoveryTime; } set { _currentRecoveryTime = value; }}
    public bool IsPerfomingAction { get { return _isPerformingAction; } set { _isPerformingAction = value; }}
    
    public CharacterStats CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; }}

    public States CurrentState { get { return _currentState; } set { _currentState = value; }}
    
    public Rigidbody EnemyRb { get { return _enemyRb; } set { _enemyRb = value; }}
    public NavMeshAgent EnemyNavMeshAgent { get { return _navMeshAgent; } set { _navMeshAgent = value; }}
    
    public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; }}
    public float MaximumAttackRange { get { return _maximumAttackRange; } set { _maximumAttackRange = value; }}

    #endregion

    private void Awake() 
    {
        _enemyController = GetComponent<EnemyLocomotion>();
        _enemyAnimation = GetComponent<EnemyAnimatorController>();
        _enemyStats = GetComponent<EnemyStats>();
        _enemyRb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    private void Start() 
    {
        _navMeshAgent.enabled = false;
        _enemyRb.isKinematic = false;
    }

    private void Update() 
    {
        HandleRecoveryTimer();

        _isInteracting = _enemyAnimation.Anim.GetBool("IsInteracting");
        _enemyAnimation.Anim.SetBool("IsDead", _enemyStats.IsDead);
    }

    private void FixedUpdate() 
    {
        HandleStateMachine();    
        
    }
    private void HandleStateMachine()
    {
        if(_currentState != null)
        {
            States nextState = _currentState.Tick(this, _enemyStats, _enemyAnimation);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(States state)
    {
        _currentState = state;
    }


    private void HandleRecoveryTimer()
    {
        if(_currentRecoveryTime > 0)
        {
            _currentRecoveryTime -= Time.deltaTime;
        }
        if(_isPerformingAction)
        {
            if(_currentRecoveryTime <= 0)
            {
                _isPerformingAction = false;
            }
        }
    }

}
