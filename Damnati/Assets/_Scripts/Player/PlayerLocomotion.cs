using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Components")]
    [Space(15)]
    private InputHandler _inputHandler;
    private UIManager _UIManager;
    [SerializeField] private Rigidbody _rb;
    private AnimatorHandler _animatorHandler;
    private PlayerManager _playerManager;
    private PlayerInventory _playerInventory;
    private PlayerAttacker _playerAttack;
    private WeaponSlotManager _weaponSlotManager;
    private PlayerStats _playerStats;

    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;

    private Transform _myTransform;

    [Header("Camera Components")]
    [Space(15)]
    private Transform _cameraRoot;
    private Vector3 _newCamRotation;

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
    private Vector3 _projectedVelocity;
    
    [Header("Ground, Checks & Tags")]
    [Space(15)]
    [SerializeField] private float _groundDetectionRayStartPoint = 0.5f;
    [SerializeField] private float _minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private float _groundDirectionRayDistance = 0.2f;
    [SerializeField] private LayerMask _ignoreForGroundCheck;

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
    #endregion

    private void Awake() 
    {
        _inputHandler = FindObjectOfType<InputHandler>();
        _playerManager = GetComponent<PlayerManager>();
        _playerStats = GetComponent<PlayerStats>();
        _animatorHandler = GetComponent<AnimatorHandler>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerAttack = GetComponent<PlayerAttacker>();
        _UIManager = FindObjectOfType<UIManager>();
        _weaponSlotManager = GetComponent<WeaponSlotManager>();

        _playerManager.IsGrounded = true;    

        _cameraRoot = Camera.main.transform;
        _myTransform = transform;

        _newCamRotation = _cameraRoot.localRotation.eulerAngles;

        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
    }

    #region Player Actions

    public void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = _inputHandler.MoveAmount;

        targetDir = _cameraRoot.forward * _inputHandler.VerticalMovement;
        targetDir += _cameraRoot.right * _inputHandler.HorizontalMovement;

        targetDir.Normalize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero)
        {
            targetDir = _myTransform.forward;
        }

        float rs = _rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, tr, rs * delta);

        _myTransform.rotation = targetRotation;
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
            _playerStats.RunStaminaDrain(_sprintStaminaCost);
        }
        else
        {
            _movDirection *= speed;
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_movDirection, _normalVector);
        _rb.velocity = projectedVelocity;

        _animatorHandler.UpdateAnimatorValues(_inputHandler.MoveAmount, 0, _playerManager.IsSprinting);

        if(_animatorHandler.CanRot)
        {
            HandleRotation(delta);
        }
    }
    public void HandleDodge(float delta)
    {
        if(_animatorHandler.Anim.GetBool("IsInteracting") || !_animatorHandler.HasAnimator || _playerStats.CurrentStamina <= 0)
        {
            return;
        }

        if(_inputHandler.SBFlag)
        {
            _movDirection = _cameraRoot.forward * _inputHandler.VerticalMovement;
            _movDirection += _cameraRoot.right * _inputHandler.HorizontalMovement;

            if(_inputHandler.MoveAmount > 0 && _playerStats.CurrentStamina >= _rollStaminaCost)
            {
                _animatorHandler.PlayTargetAnimation("Roll", true);
                _movDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(_movDirection);
                _myTransform.rotation = rollRotation;
                _playerStats.StaminaDrain(_rollStaminaCost);
            }
            else if(_playerStats.CurrentStamina >= _backstepStaminaCost)
            {
                _animatorHandler.PlayTargetAnimation("Backstep", true);
                _playerStats.StaminaDrain(_backstepStaminaCost);
            }
        }
    }
    public void HandleGravity(float delta, Vector3 moveDirection)
    {
        _playerManager.IsGrounded = false;
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += _groundDetectionRayStartPoint;

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
        if (Physics.Raycast(origin, -Vector3.up, out hit, _minimumDistanceNeededToBeginFall, _ignoreForGroundCheck))
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
                    _animatorHandler.PlayTargetAnimation("Land", true);
                    _inAirTimer = 0;
                }
                else
                {
                    _animatorHandler.PlayTargetAnimation("Empty", false);
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
                    _animatorHandler.PlayTargetAnimation("Fall", true);
                }

                Vector3 vel = _rb.velocity;
                vel.Normalize();
                _rb.velocity = vel * (_movSpeed / 2);
                _playerManager.IsInAir = true;
            }
        }

        if (_playerManager.IsInteracting || _inputHandler.MoveAmount > 0)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            transform.position = _targetPosition;
        }
    }
    public void HandleTwoWeapon(float delta)
    {
        if(_inputHandler.THEquipFlag)
        {
            _inputHandler.THEquipFlag = false;
            _playerManager.TwoHandFlag = !_playerManager.TwoHandFlag;

            if(_playerManager.TwoHandFlag)
            {
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightHandWeapon, false);
            }
            else
            {
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightHandWeapon, false);
                _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.leftHandWeapon, true);
            }
        }
    }
    #endregion
}
