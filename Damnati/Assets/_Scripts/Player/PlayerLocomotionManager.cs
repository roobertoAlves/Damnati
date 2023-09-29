using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager _player;

    [Header("Player Movement Stats")]
    [Space(15)]
    [SerializeField] private int _runSpeed = 7;
    [SerializeField] private int _movSpeed = 5;
    [SerializeField] private int _rotationSpeed = 10;

    [Header("Stamina Costs")]
    [Space(15)]

    [SerializeField] private int _rollStaminaCost = 15;
    [SerializeField] private int _backstepStaminaCost = 8; 
    [SerializeField] private float _sprintStaminaCost = 0.5f;


    protected override void Awake() 
    {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }
    protected override void Start() 
    {
      
    }
    protected override void Update()
    {
        base.Update();
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
                    if (_player.IsSprinting || _player.PlayerInput.DogdeFlag)
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
                        Vector3 rotationDirection = MoveDirection;
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
    public void HandleGroundedMovement()
    {
        if(_player.PlayerInput.DogdeFlag || _player.IsInteracting || !_player.IsGrounded)
        {
            return;
        }

        MoveDirection = _player.PlayerCamera.transform.forward * _player.PlayerInput.VerticalMovement;
        MoveDirection = MoveDirection + _player.PlayerCamera.transform.right * _player.PlayerInput.HorizontalMovement;
        MoveDirection.Normalize();
        MoveDirectionY = 0;

        if(_player.IsSprinting && _player.PlayerInput.MoveAmount > 0.5f) 
        {
            _player.CharacterController.Move(MoveDirection * _runSpeed * Time.deltaTime);
            _player.PlayerStats.DeductSprintingStamina(_sprintStaminaCost);
        }
        else
        {
            _player.CharacterController.Move(MoveDirection * _movSpeed * Time.deltaTime);
        }


        if(_player.PlayerInput.LockOnFlag && _player.IsSprinting == false)
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
            || _player.PlayerStats.CurrentStamina <= 0)
        {
            return;
        }

        if(_player.PlayerInput.DogdeFlag)
        {
            _player.PlayerInput.DogdeFlag = false; 

            MoveDirection = _player.PlayerCamera.CameraObject.transform.forward * _player.PlayerInput.VerticalMovement;
            MoveDirection += _player.PlayerCamera.CameraObject.transform.right * _player.PlayerInput.HorizontalMovement;

            if(_player.PlayerInput.MoveAmount > 0 && _player.PlayerStats.CurrentStamina >= _rollStaminaCost)
            {
                _player.PlayerAnimator.PlayTargetAnimation("Roll", true); 
                _player.PlayerAnimator.EraseHandIKForWeapon();
                MoveDirectionY = 0;
                Quaternion rollRotation = Quaternion.LookRotation(MoveDirection);
                transform.rotation = rollRotation;
                _player.PlayerStats.DeductStamina(_rollStaminaCost);
            }
            else if(_player.PlayerStats.CurrentStamina >= _backstepStaminaCost)
            {
                _player.PlayerAnimator.PlayTargetAnimation("Backstep", true);
                _player.PlayerAnimator.EraseHandIKForWeapon();
                _player.PlayerStats.DeductStamina(_backstepStaminaCost);
            }
        }
    }
   

    #endregion
}
