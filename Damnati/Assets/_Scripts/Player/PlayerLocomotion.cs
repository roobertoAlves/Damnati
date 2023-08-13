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
    [SerializeField] private int _runSpeed;
    [SerializeField] private int _movSpeed;
    [SerializeField] private int _walkSpeed;
    [SerializeField] private int _rotationSpeed = 10;

    private Vector3 _movDirection;
    private Vector3 _targetPosition;
    private Vector3 _normalVector;
    private Vector3 _projectedVelocity;
    
    [Header("Ground, Checks & Tags")]
    [Space(15)]
    [SerializeField] private float _minimumDistanceToFall = 1f;
    [SerializeField] private LayerMask _ignoreForGroundCheck;
    [SerializeField] private GameObject _leftFootRoot;
    [SerializeField] private GameObject _rightFootRoot;
    [SerializeField] private Vector3 _leftFootOffset;
    [SerializeField] private Vector3 _rightFootOffset;
    [SerializeField] private Vector3 _centerOffset;

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
        _animatorHandler = GetComponent<AnimatorHandler>();
        _playerManager.IsGrounded = true;    

        _cameraRoot = Camera.main.transform;
        _myTransform = transform;

        _newCamRotation = _cameraRoot.localRotation.eulerAngles;

        Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
    }

    #region Player Actions

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = _inputHandler.MoveAmount;

        targetDir = _cameraRoot.forward * _inputHandler.VerticalMovement;
        targetDir = _cameraRoot.right * _inputHandler.HorizontalMovement;

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

        if(_inputHandler.RunFlag)
        {
            speed = _runSpeed;
            _playerManager.IsSprinting = true; 
            _movDirection *= speed;
        }
        else
        {
            if(_inputHandler.MoveAmount < 0.5)
            {
                _movDirection *= _walkSpeed;
            }
            else
            {
                _movDirection *= speed;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(_movDirection, _normalVector);
        _rb.velocity = projectedVelocity;

        if(_animatorHandler.CanRot)
        {
            HandleRotation(delta);
        }
    }
    public void HandleDodge(float delta)
    {
        if(_animatorHandler.Anim.GetBool("IsInteracting"))
        {
            return;
        }

        if(_inputHandler.SBFlag)
        {
            _movDirection = _cameraRoot.forward * _inputHandler.VerticalMovement;
            _movDirection += _cameraRoot.right * _inputHandler.HorizontalMovement;

            if(_inputHandler.MoveAmount > 0)
            {
                _animatorHandler.PlayTargetAnimation("Roll", true);
                _movDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(_movDirection);
                _myTransform.rotation = rollRotation;
            }
            else
            {
                _animatorHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }
    public void HandleGravity(float delta, Vector3 _movDirection)
    {
        if (!_animatorHandler.HasAnimator)
        {
            return;
        }

        _playerManager.IsGrounded = false;

        _targetPosition = transform.position;

        bool isCenterGrounded = Physics.Raycast(_myTransform.position + _centerOffset, -Vector3.up, out RaycastHit centerHit, _minimumDistanceToFall, _ignoreForGroundCheck);
        bool isRightFootGrounded = Physics.Raycast(_rightFootRoot.transform.position + _rightFootOffset, -Vector3.up, out RaycastHit rightFootHit, _minimumDistanceToFall, _ignoreForGroundCheck);
        bool isLeftFootGrounded = Physics.Raycast(_leftFootRoot.transform.position + _leftFootOffset, -Vector3.up, out RaycastHit leftFootHit, _minimumDistanceToFall, _ignoreForGroundCheck);

        bool isAnyFootGrounded = isCenterGrounded || isRightFootGrounded || isLeftFootGrounded;
        Debug.Log("Is Grounded: " + isAnyFootGrounded);

        if(!isAnyFootGrounded)
        {
            _movDirection = Vector3.zero;
        }

        if(_playerManager.IsInAir)
        {
            _rb.AddForce(-Vector3.up * _fallingSpeed);
            _rb.AddForce(_movDirection * _fallingSpeed / 10f);
        }

    if(isAnyFootGrounded)
    {
        _playerManager.IsGrounded = true;

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
                    _animatorHandler.PlayTargetAnimation("Locomotion", false);
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

        if(_playerManager.IsGrounded)
        {
            if(_playerManager.IsInteracting || _inputHandler.MoveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime);
            }
            else
            {
                transform.position = _targetPosition;
            }
        }

    }
    public void HandleAttack(float delta)
    {
        if (!_animatorHandler.HasAnimator)
        {
            return;
        }

        if (_inputHandler.LBAttackFlag)
        {
            if (_playerManager.CanDoCombo)
            {
                _inputHandler.ComboFlag = true;
                _playerAttack.HandleWeaponCombo(_playerInventory.rightHandWeapon);
                _inputHandler.ComboFlag = false;
            }
            else
            {
                if (_playerManager.IsInteracting || _playerManager.CanDoCombo)
                {
                    return;
                }

                _animatorHandler.Anim.SetBool("IsUsingRightHand", true);
                _playerAttack.HandleLightAttack(_playerInventory.rightHandWeapon);
            }
        }
        if (_inputHandler.RBAttackFlag)
        {
            _animatorHandler.Anim.SetBool("IsUsingRightHand", true);
            _playerAttack.HandleHeavyAttack(_playerInventory.rightHandWeapon);
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

    private void OnDrawGizmos()
    {
        RaycastHit hitLeftFoot;
        RaycastHit hitRightFoot;
        RaycastHit hitCenter;

        Vector3 rightFootOrigin = _rightFootRoot.transform.position + _rightFootOffset;
        Vector3 leftFootOrigin = _leftFootRoot.transform.position + _leftFootOffset;
        Vector3 centerOrigin = transform.position + _centerOffset;


        bool isRightFootGrounded = Physics.Raycast(rightFootOrigin, -Vector3.up, out hitRightFoot, _minimumDistanceToFall, _ignoreForGroundCheck);
        bool isLeftFootGrounded = Physics.Raycast(leftFootOrigin, -Vector3.up, out hitLeftFoot, _minimumDistanceToFall, _ignoreForGroundCheck);
        bool isCenterGrounded = Physics.Raycast(centerOrigin, -Vector3.up, out hitCenter, _minimumDistanceToFall, _ignoreForGroundCheck);

        Gizmos.color = Color.red;

        if (isRightFootGrounded)
        Gizmos.DrawLine(rightFootOrigin, hitRightFoot.point);

        if (isLeftFootGrounded)
        Gizmos.DrawLine(leftFootOrigin, hitLeftFoot.point);

        Gizmos.color = Color.red;

        if (isCenterGrounded)
        Gizmos.DrawLine(centerOrigin, hitCenter.point);
    }

    #endregion

}
