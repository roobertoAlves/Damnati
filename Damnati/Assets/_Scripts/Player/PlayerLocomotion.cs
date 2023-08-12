using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Components & Scripts")]
    [Space(15)]

    private InputHandler _inputHandler;
    private AnimatorHandler _animatorHandler;
    private PlayerManager _playerManager;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private CapsuleCollider _characterCollider;
    [SerializeField] private CapsuleCollider _characterCollisionBlockerCollider;
    private Transform _myTransform;

    [Header("Camera Components")]
    [Space(15)]
    [SerializeField] private Transform _cameraRoot;
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
    [SerializeField] private LayerMask _ignoreForGroundCheck;
    [SerializeField] private GameObject _leftFootRoot;
    [SerializeField] private GameObject _rightFootRoot;
    [SerializeField] private Vector3 _leftFootOffset;
    [SerializeField] private Vector3 _rightFootOffset;
    [SerializeField] private Vector2 _centerOffset;

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
        _newCamRotation = _cameraRoot.localRotation.eulerAngles;
        //_playerManager.IsGrounded = true;    
        _myTransform = transform;

        //Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollider, true);
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

    public void HandleRollingAndSprinting(float delta)
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
                _animatorHandler.PlayTargetAnimation("Rolling", true);
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

    #endregion

}
