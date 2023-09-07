using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    [Header("Components")]
    [Space(15)]
    private Rigidbody _rb;
    private InputHandler _inputHandler;
    private PlayerAnimatorManager _playerAnimatorManager;
    private PlayerManager _playerManager;
    private PlayerStatsManager _playerStatsManager;
    private CameraHandler _cameraHandler;

    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;

    private Transform _myTransform;

    [Header("Critical Attack Components")]
    [Space(15)]
    [SerializeField] private Transform _criticalAttackRayCastStartPoint;

    [Header("Camera Components")]
    [Space(15)]
    private Transform _cameraRoot;

    [Header("Gravity Parameters")]
    [Space(15)]
    [SerializeField] private float _fallingSpeed;
    private float _inAirTimer;

    [Header("Player Movement Stats")]
    [Space(15)]
    [SerializeField] private int _runSpeed = 7;
    [SerializeField] private int _movSpeed = 5;
    [SerializeField] private int _rotationSpeed = 10;

    private Vector3 _movDirection;
    private Vector3 _targetPosition;
    private Vector3 _normalVector;
    
    [Header("Ground, Checks & Tags")]
    [Space(15)]
    [SerializeField] private float _groundDetectionRayStartPoint = 0.5f;
    [SerializeField] private float _minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private float _groundDirectionRayDistance = 0.2f;
    [SerializeField] private LayerMask _ignoreForGroundCheck;

    [SerializeField] private GameObject _leftFoot;
    [SerializeField] private GameObject _rightFoot;

    [Header("Stamina Costs")]
    [Space(15)]

    [SerializeField] private int _rollStaminaCost = 15;
    [SerializeField] private int _backstepStaminaCost = 8; 
    [SerializeField] private float _sprintStaminaCost = 0.5f;

    #region GET & SET

    public Rigidbody PlayerRB { get { return _rb; } set { _rb = value; }}
    public LayerMask IgnoreForGroundCheck { get { return _ignoreForGroundCheck;} set { _ignoreForGroundCheck = value; }}
    public float InAirTimer { get { return _inAirTimer; } set { _inAirTimer = value; }}
    public Vector3 MoveDirection { get { return _movDirection; } set { _movDirection = value; }}
    public float MoveDirectionY { get { return _movDirection.y; } set { _movDirection.y = value; }}
    
    public Transform CriticalAttackRayCastStartPoint { get { return _criticalAttackRayCastStartPoint; } set { _criticalAttackRayCastStartPoint = value; }}
    #endregion

    private void Awake() 
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerManager = GetComponent<PlayerManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start() 
    {
        _cameraRoot = Camera.main.transform;
        _myTransform = transform;
        _playerAnimatorManager.Initialize();

        _playerManager.IsGrounded = true;
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);    
    }

    #region Player Actions

    public void HandleRotation(float delta)
    {
        if (_playerAnimatorManager.canRotate)
        {
            if(_inputHandler.LockOnFlag)
            {
                if (_inputHandler.RunFlag || _inputHandler.SBFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = _cameraHandler.CameraTransform.forward * _inputHandler.VerticalMovement;
                    targetDirection += _cameraHandler.CameraTransform.right * _inputHandler.HorizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, _rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = _movDirection;
                    rotationDirection = _cameraHandler.CurrentLockOnTarget.transform.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, _rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = _inputHandler.MoveAmount;

                targetDir = _cameraRoot.forward * _inputHandler.VerticalMovement;
                targetDir += _cameraRoot.right * _inputHandler.HorizontalMovement;

                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                {
                    targetDir = _myTransform.forward;
                }

                float rs = _rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, tr, rs * delta);

                _myTransform.rotation = targetRotation;
            }
        }
    }
    public void HandleMovement(float delta)
    {
        if(_inputHandler.SBFlag || _playerManager.IsInteracting)
        {
            return;
        }

        _movDirection = _cameraRoot.forward * _inputHandler.VerticalMovement;
        _movDirection += _cameraRoot.right * _inputHandler.HorizontalMovement;
        _movDirection.Normalize();
        _movDirection.y = 0;

        float speed = _movSpeed;

        if(_inputHandler.RunFlag && _inputHandler.MoveAmount > 0.5f)
        {
            speed = _runSpeed;
            _playerManager.IsSprinting = true; 
            _movDirection *= speed;
            _playerStatsManager.RunStaminaDrain(_sprintStaminaCost);
        }
        else
        {
            _movDirection *= speed;
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_movDirection, _normalVector);
        _rb.velocity = projectedVelocity;

        if(_inputHandler.LockOnFlag && _inputHandler.RunFlag == false)
        {
            _playerAnimatorManager.UpdateAnimatorValues(_inputHandler.VerticalMovement, _inputHandler.HorizontalMovement, _playerManager.IsSprinting);
        }
        else
        {
            _playerAnimatorManager.UpdateAnimatorValues(_inputHandler.MoveAmount, 0, _playerManager.IsSprinting);
        }
    }
    public void HandleDodge(float delta)
    {
        if(_playerAnimatorManager.Anim.GetBool("IsInteracting") || !_playerAnimatorManager.HasAnimator || _playerStatsManager.CurrentStamina <= 0)
        {
            return;
        }

        if(_inputHandler.SBFlag)
        {
            _movDirection = _cameraRoot.forward * _inputHandler.VerticalMovement;
            _movDirection += _cameraRoot.right * _inputHandler.HorizontalMovement;

            if(_inputHandler.MoveAmount > 0 && _playerStatsManager.CurrentStamina >= _rollStaminaCost)
            {
                _playerAnimatorManager.PlayTargetAnimation("Roll", true); 
                _movDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(_movDirection);
                _myTransform.rotation = rollRotation;
                _playerStatsManager.StaminaDrain(_rollStaminaCost);
            }
            else if(_playerStatsManager.CurrentStamina >= _backstepStaminaCost)
            {
                _playerAnimatorManager.PlayTargetAnimation("Backstep", true);
                _playerStatsManager.StaminaDrain(_backstepStaminaCost);
            }
        }
    }
    public void HandleGravity(float delta, Vector3 moveDirection)
    {
        _playerManager.IsGrounded = false;
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += _groundDetectionRayStartPoint;

        Vector3 leftFootOrigin = _leftFoot.transform.position;
        leftFootOrigin.y += _groundDetectionRayStartPoint;

        Vector3 rightFootOrigin = _rightFoot.transform.position;
        rightFootOrigin.y += _groundDetectionRayStartPoint;

        if(Physics.Raycast(origin, transform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if(_playerManager.IsInAir)
        {
            _rb.AddForce(-Vector3.up * _fallingSpeed);
            _rb.AddForce(moveDirection * _fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * _groundDirectionRayDistance;
        


        _targetPosition = transform.position;

        Debug.DrawRay(origin, -Vector3.up * _minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        Debug.DrawRay(leftFootOrigin, -Vector3.up * _minimumDistanceNeededToBeginFall, Color.blue, 0.1f, false);
        Debug.DrawRay(rightFootOrigin, -Vector3.up * _minimumDistanceNeededToBeginFall, Color.blue, 0.1f, false);
        
        if (Physics.Raycast(origin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck) ||
            Physics.Raycast(leftFootOrigin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck) ||
            Physics.Raycast(rightFootOrigin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
        {
            _normalVector = hit.normal;
            Vector3 tp = hit.point;
            _playerManager.IsGrounded = true;
            _targetPosition.y = tp.y;

            if(_playerManager.IsInAir)
            {
                if(_inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + _inAirTimer);
                    _playerAnimatorManager.PlayTargetAnimation("Land", true);
                    _playerManager.IsInteracting = true;
                    _inAirTimer = 0;
                }
                else
                {
                   _playerManager.IsInteracting = false;
                   _playerAnimatorManager.PlayTargetAnimation("Empty", false);
                    _inAirTimer = 0;
                }

                _playerManager.IsInAir = false;
            }
        }
        else
        {
            if(_playerManager.IsGrounded)
            {
                _playerManager.IsGrounded = false;
            }

            if(_playerManager.IsInAir == false)
            {
                if(_playerManager.IsInteracting == false)
                {
                    _playerAnimatorManager.PlayTargetAnimation("Fall", true);
                }

                Vector3 vel = _rb.velocity;
                vel.Normalize();
                _rb.velocity = vel * (_movSpeed / 2);
                _playerManager.IsInAir = true;
            }
        }

        if (_playerManager.IsInteracting || _inputHandler.MoveAmount > 0)
        {
            _myTransform.position = Vector3.Lerp(_myTransform.position, _targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            _myTransform.position = _targetPosition;
        }
    }

    #endregion
}
