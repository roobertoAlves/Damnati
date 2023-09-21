using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : CharacterManager
{
    [Header("A.I Scripts Components")]
    [Space(15)]
    private EnemyBossManager _enemyBossManager;
    private EnemyLocomotionManager _enemyLocomotionManager;
    private EnemyAnimatorManager _enemyAnimatorManager;
    private EnemyStatsManager _enemyStatsManager;
    private EnemyEffectsManager _enemyEffectsManager;

    [SerializeField] private States _currentState;
    [SerializeField] private CharacterManager _currentTarget;

    private bool _isPerformingAction;


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
    [SerializeField] private float _maximumAggroRadius = 5f;

    [Header("A.I Combat Settings")]
    [Space(15)]
    [SerializeField] private bool _allowAIToPerformCombos;
    [SerializeField] private bool _isPhaseShifting;
    [SerializeField] private float _comboLikelyHood;

    [Header("A.I Target Information")]
    [Space(15)]
    [SerializeField] private float _distanceFromTarget;
    [SerializeField] private float _viewableAngle; 
    [SerializeField] private Vector3 _targetsDirection;

    #region Get & Set
    public EnemyBossManager EnemyBossManager { get { return _enemyBossManager; }}
    public EnemyLocomotionManager EnemyLocomotionManager { get { return _enemyLocomotionManager; }}
    public EnemyAnimatorManager EnemyAnimatorManager { get { return _enemyAnimatorManager; }}
    public EnemyStatsManager EnemyStatsManager { get { return _enemyStatsManager; }}
    public EnemyEffectsManager EnemyEffectsManager { get { return _enemyEffectsManager; }}

    public float DetectionRadius { get { return _detectionRadius; } set { _detectionRadius = value; }}

    public float MinimumDetectionAngle {get { return _minimumDetectionAngle; } set { _minimumDetectionAngle = value; }}
    public float MaximumDetectionAngle { get { return _maximumDetectionAngle; } set { _maximumDetectionAngle = value; }}
    
    public float CurrentRecoveryTime { get { return _currentRecoveryTime; } set { _currentRecoveryTime = value; }}
    public bool IsPerfomingAction { get { return _isPerformingAction; } set { _isPerformingAction = value; }}
    public bool AllowAIToPerformCombos { get { return _allowAIToPerformCombos; } set { _allowAIToPerformCombos = value; }}
    public bool IsPhaseShifting { get { return _isPhaseShifting; } set { _isPhaseShifting = value; }}

    public CharacterManager CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; }}

    public States CurrentState { get { return _currentState; } set { _currentState = value; }}
    
    public Rigidbody EnemyRb { get { return _enemyRb; } set { _enemyRb = value; }}
    public NavMeshAgent EnemyNavMeshAgent { get { return _navMeshAgent; } set { _navMeshAgent = value; }}
    
    public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; }}
    public float MaximumAggroRadius { get { return _maximumAggroRadius; } set { _maximumAggroRadius = value; }}
    public float ComboLikelyHood { get { return _comboLikelyHood; } set { _comboLikelyHood = value; }}
    
    public float DistanceFromTarget { get { return _distanceFromTarget; } set { _distanceFromTarget = value; }}
    public float ViewableAngle { get { return _viewableAngle; } set { _viewableAngle = value; }}
    public Vector3 TargetsDirection { get { return _targetsDirection; } set { _targetsDirection = value; }}
    #endregion

    protected override void Awake() 
    {
        base.Awake();
        _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        _enemyBossManager = GetComponent<EnemyBossManager>();
        _enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        _enemyStatsManager = GetComponent<EnemyStatsManager>();
        _enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        _enemyRb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        _navMeshAgent.enabled = false;
    }

    private void Start() 
    {
        _enemyRb.isKinematic = false;
    }

    private void Update() 
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        
        IsRotatingWithRootMotion = Animator.GetBool("IsRotatingWithRootMotion");
        IsInteracting = Animator.GetBool("IsInteracting");
        _isPhaseShifting = Animator.GetBool("IsPhaseShifting");
        IsInvulnerable = Animator.GetBool("IsInvulnerable");
        CanDoCombo = Animator.GetBool("CanDoCombo");
        CanRotate = Animator.GetBool("CanRotate");
        Animator.SetBool("IsDead", IsDead);

        if(_currentTarget != null)
        {
            _distanceFromTarget = Vector3.Distance(_currentTarget.transform.position, transform.position);
            _targetsDirection = _currentTarget.transform.position - transform.position;
            _viewableAngle = Vector3.Angle(_targetsDirection,  transform.forward);

        }
    }
    protected override void FixedUpdate() 
    {
        base.FixedUpdate();
        _enemyEffectsManager.HandleAllBuildUpEffects();    
    }
    private void LateUpdate()
    {
        _navMeshAgent.transform.localPosition = Vector3.zero;
        _navMeshAgent.transform.localRotation = Quaternion.identity; 
        
    }
    private void HandleStateMachine()
    {
        if(_currentState != null)
        {
            States nextState = _currentState.Tick(this);

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
