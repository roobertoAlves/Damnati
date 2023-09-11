using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : CharacterManager
{
    [Header("A.I Scripts Components")]
    [Space(15)]
    private EnemyLocomotionManager _enemyLocomotionManager;
    private EnemyAnimatorManager _enemyAnimatorManager;
    private EnemyStatsManager _enemyStatsManager;
    private EnemyEffectsManager _enemyEffectsManager;

    [SerializeField] private States _currentState;
    [SerializeField] private CharacterStatsManager _currentTarget;

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

    #region Get & Set

    public float DetectionRadius { get { return _detectionRadius; } set { _detectionRadius = value; }}

    public float MinimumDetectionAngle {get { return _minimumDetectionAngle; } set { _minimumDetectionAngle = value; }}
    public float MaximumDetectionAngle { get { return _maximumDetectionAngle; } set { _maximumDetectionAngle = value; }}
    
    public float CurrentRecoveryTime { get { return _currentRecoveryTime; } set { _currentRecoveryTime = value; }}
    public bool IsPerfomingAction { get { return _isPerformingAction; } set { _isPerformingAction = value; }}
    public bool AllowAIToPerformCombos { get { return _allowAIToPerformCombos; } set { _allowAIToPerformCombos = value; }}
    public bool IsPhaseShifting { get { return _isPhaseShifting; } set { _isPhaseShifting = value; }}

    public CharacterStatsManager CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; }}

    public States CurrentState { get { return _currentState; } set { _currentState = value; }}
    
    public Rigidbody EnemyRb { get { return _enemyRb; } set { _enemyRb = value; }}
    public NavMeshAgent EnemyNavMeshAgent { get { return _navMeshAgent; } set { _navMeshAgent = value; }}
    
    public float RotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; }}
    public float MaximumAggroRadius { get { return _maximumAggroRadius; } set { _maximumAggroRadius = value; }}
    public float ComboLikelyHood { get { return _comboLikelyHood; } set { _comboLikelyHood = value; }}
    #endregion

    private void Awake() 
    {
        _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
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
        
        IsUsingLeftHand = _enemyAnimatorManager.Anim.GetBool("IsUsingLeftHand");
        IsUsingRightHand = _enemyAnimatorManager.Anim.GetBool("IsUsingRightHand");
        IsRotatingWithRootMotion = _enemyAnimatorManager.Anim.GetBool("IsRotatingWithRootMotion");
        IsInteracting = _enemyAnimatorManager.Anim.GetBool("IsInteracting");
        _isPhaseShifting = _enemyAnimatorManager.Anim.GetBool("IsPhaseShifting");
        IsInvulnerable = _enemyAnimatorManager.Anim.GetBool("IsInvulnerable");
        CanDoCombo = _enemyAnimatorManager.Anim.GetBool("CanDoCombo");
        CanRotate = _enemyAnimatorManager.Anim.GetBool("CanRotate");
        _enemyAnimatorManager.Anim.SetBool("IsDead", _enemyStatsManager.IsDead);
    }
    private void FixedUpdate() 
    {
        //_enemyEffectsManager.HandleAllBuildUpEffects();    
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
            States nextState = _currentState.Tick(this, _enemyStatsManager, _enemyAnimatorManager);

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
