using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private AnimatorHandler _animatorHandler;
    //private CameraHandler _cameraHandler;
    private PlayerLocomotion _playerLocomotion;

    #region Player Flags

    private bool _isInteracting;
    private bool _isInAir;
    private bool _isGrounded;
    private bool _canDoCombo;
    private bool _isRollingOrSteppingBack;
    private bool _isAttacking;
    private bool _twoHandFlag;
    private bool _isUsingLeftHand;
    private bool _isUsingRightHand;
    private bool _isInvulnerable;
    private bool _isSprinting;

    #endregion

    #region GET & SET

    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; }}
    public bool IsInteracting { get { return _isInteracting; } set { _isInteracting = value; }}
    public bool IsInAir { get { return _isInAir; } set { _isInAir = value; }}
    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; }}

    #endregion  

    private void Awake() 
    {
        
    }

    private void Start() 
    {
        
    }

    private void Update() 
    {   
        float delta = Time.deltaTime;

        _isInteracting = _animatorHandler.Anim.GetBool("IsInteracting");
        _canDoCombo = _animatorHandler.Anim.GetBool("CanDoCombo");
        _isUsingLeftHand = _animatorHandler.Anim.GetBool("IsUsingLeftHand");
        _isUsingRightHand = _animatorHandler.Anim.GetBool("IsUsingRightHand");
        _isInvulnerable = _animatorHandler.Anim.GetBool("IsInvulnerable");

        _inputHandler.TickInput(delta);
        _playerLocomotion.HandleMovement(delta);
        _playerLocomotion.HandleRollingAndSprinting(delta);
        //_playerLocomotion.HandleGravity(delta,_playerLocomotion.MoveDirection);
        
    }

    private void LateUpdate() 
    {
        if(_isInAir)
        {
            _playerLocomotion.InAirTimer = _playerLocomotion.InAirTimer + Time.deltaTime;
        }    
    }
}
