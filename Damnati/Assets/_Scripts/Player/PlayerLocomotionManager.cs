using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    [Header("Components")]
    [Space(15)]
    private Rigidbody _rb;
    private PlayerManager _player;

    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;

    [Header("Critical Attack Components")]
    [Space(15)]
    [SerializeField] private Transform _criticalAttackRayCastStartPoint;


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
   
    public CapsuleCollider CharacterCollider { get { return _characterCollider; } set { _characterCollider = value; }}
    public CapsuleCollider CharacterCollisionBlockerCollider { get { return _characterCollisionBlockerCollider; } set { _characterCollisionBlockerCollider = value; }}
    #endregion

    private void Awake() 
    {
        _player = GetComponent<PlayerManager>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start() 
    {
        _player.IsGrounded = true;
        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);    
    }

    #region Player Actions

    public void HandleRotation()
    {
        if (_player.CanRotate)
        {
            if (_player.IsAiming)
            {
                Quaternion targetRotation = Quaternion.Euler(0, _player.PlayerCamera.CameraTransform.eulerAngles.y, 0);
                Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
            else
            {
                if (_player.PlayerInput.LockOnFlag)
                {
                    if (_player.PlayerInput.RunFlag || _player.PlayerInput.SBFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = _player.PlayerCamera.CameraTransform.forward * _player.PlayerInput.VerticalMovement;
                        targetDirection += _player.PlayerCamera.CameraTransform.right * _player.PlayerInput.HorizontalMovement;
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
                        rotationDirection = _player.PlayerCamera.CurrentLockOnTarget.transform.position - transform.position;
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
                    float moveOverride = _player.PlayerInput.MoveAmount;

                    targetDir = _player.PlayerCamera.CameraObject.transform.forward * _player.PlayerInput.VerticalMovement;
                    targetDir += _player.PlayerCamera.CameraObject.transform.right * _player.PlayerInput.HorizontalMovement;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                    {
                        targetDir = transform.forward;
                    }

                    float rs = _rotationSpeed;

                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
            }
        }
    }
    public void HandleMovement()
    {
        if(_player.PlayerInput.SBFlag || _player.IsInteracting)
        {
            return;
        }

        _movDirection = _player.PlayerCamera.CameraObject.transform.forward * _player.PlayerInput.VerticalMovement;
        _movDirection += _player.PlayerCamera.CameraObject.transform.right * _player.PlayerInput.HorizontalMovement;
        _movDirection.Normalize();
        _movDirection.y = 0;

        float speed = _movSpeed;

        if(_player.PlayerInput.RunFlag && _player.PlayerInput.MoveAmount > 0.5f)
        {
            speed = _runSpeed;
            _player.IsSprinting = true; 
            _movDirection *= speed;
            _player.PlayerStats.RunStaminaDrain(_sprintStaminaCost);
        }
        else
        {
            _movDirection *= speed;
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_movDirection, _normalVector);
        _rb.velocity = projectedVelocity;

        if(_player.PlayerInput.LockOnFlag && _player.PlayerInput.RunFlag == false)
        {
            _player.PlayerAnimator.UpdateAnimatorValues(_player.PlayerInput.VerticalMovement, _player.PlayerInput.HorizontalMovement, _player.IsSprinting);
        }
        else
        {
            _player.PlayerAnimator.UpdateAnimatorValues(_player.PlayerInput.MoveAmount, 0, _player.IsSprinting);
        }
    }
    public void HandleDodge()
    {
        if(_player.Animator.GetBool("IsInteracting") 
            || !_player.PlayerAnimator.HasAnimator 
            || _player.PlayerStats.CurrentStamina <= 0)
        {
            return;
        }

        if(_player.PlayerInput.SBFlag)
        {
            _player.PlayerInput.SBFlag = false; 

            _movDirection = _player.PlayerCamera.CameraObject.transform.forward * _player.PlayerInput.VerticalMovement;
            _movDirection += _player.PlayerCamera.CameraObject.transform.right * _player.PlayerInput.HorizontalMovement;

            if(_player.PlayerInput.MoveAmount > 0 && _player.PlayerStats.CurrentStamina >= _rollStaminaCost)
            {
                _player.PlayerAnimator.PlayTargetAnimation("Roll", true); 
                _player.PlayerAnimator.EraseHandIKForWeapon();
                _movDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(_movDirection);
                transform.rotation = rollRotation;
                _player.PlayerStats.StaminaDrain(_rollStaminaCost);
            }
            else if(_player.PlayerStats.CurrentStamina >= _backstepStaminaCost)
            {
                _player.PlayerAnimator.PlayTargetAnimation("Backstep", true);
                _player.PlayerAnimator.EraseHandIKForWeapon();
                _player.PlayerStats.StaminaDrain(_backstepStaminaCost);
            }
        }
    }
    public void HandleGravity(Vector3 moveDirection)
    {
        _player.IsGrounded = false;
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

        if(_player.IsInAir)
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
            _player.IsGrounded = true;
            _targetPosition.y = tp.y;

            if(_player.IsInAir)
            {
                if(_inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + _inAirTimer);
                    _player.PlayerAnimator.PlayTargetAnimation("Land", true);
                    _player.IsInteracting = true;
                    _inAirTimer = 0;
                }
                else
                {
                   _player.IsInteracting = false;
                   _player.PlayerAnimator.PlayTargetAnimation("Empty", false);
                    _inAirTimer = 0;
                }

                _player.IsInAir = false;
            }
        }
        else
        {
            if(_player.IsGrounded)
            {
                _player.IsGrounded = false;
            }

            if(_player.IsInAir == false)
            {
                if(_player.IsInteracting == false)
                {
                    _player.PlayerAnimator.PlayTargetAnimation("Fall", true);
                }

                Vector3 vel = _rb.velocity;
                vel.Normalize();
                _rb.velocity = vel * (_movSpeed / 2);
                _player.IsInAir = true;
            }
        }

        if (_player.IsInteracting || _player.PlayerInput.MoveAmount > 0)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            transform.position = _targetPosition;
        }
    }

    #endregion
}
