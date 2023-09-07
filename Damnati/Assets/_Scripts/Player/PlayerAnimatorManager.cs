using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : AnimatorManager
{
    private InputHandler _inputHandler;
    private PlayerStatsManager _playerStatsManager;
    private PlayerLocomotionManager _playerLocomotionManager;
    private PlayerManager _playerManager;

    private int _horizontalVelocity;
    private int _verticalVelocity;


    private bool _hasAnimator;

    #region  GET & SET
    public bool HasAnimator { get { return _hasAnimator; } set { _hasAnimator = value; }}
    
    #endregion

    public void Initialize() 
    {
        _hasAnimator = TryGetComponent<Animator>(out Anim);

        Anim = GetComponent<Animator>(); 

        _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerManager = GetComponent<PlayerManager>();
        _inputHandler = FindObjectOfType<InputHandler>();
        
        _horizontalVelocity = Animator.StringToHash("Horizontal");
        _verticalVelocity = Animator.StringToHash("Vertical");   
    }
    
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        if (isSprinting && _inputHandler.MoveAmount > 0)
        {
            v = 2;
            h = horizontalMovement;
        }

        Anim.SetFloat(_verticalVelocity, v, 0.1f, Time.deltaTime);
        Anim.SetFloat(_horizontalVelocity, h, 0.1f, Time.deltaTime);
    }
    private void OnAnimationMove()
    {
        if(!HasAnimator || _playerManager.IsInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        _playerLocomotionManager.PlayerRB.drag = 0;
        Vector3 deltaPos = Anim.deltaPosition;
        deltaPos.y = 0;
        Vector3 velocity = deltaPos / delta;
        _playerLocomotionManager.PlayerRB.velocity = velocity;
    }


    #region COMBAT
    public void EnableCombo()
    {
        Anim.SetBool("CanCombo", true);
        Debug.Log("Ativando o combo" + _playerManager.CanDoCombo);
    }

    public void DisableCombo()
    {
        Anim.SetBool("CanCombo", false);
        Debug.Log("Desativando o combo" + _playerManager.CanDoCombo);
    }

    public void EnableIsInvunerable()
    {
        Anim.SetBool("IsInvulnerable", true);
    }
    public void DisableIsInvunerable()
    {
        Anim.SetBool("IsInvulnerable", false);
    }
    public void EnableIsParrying()
    {
        _playerManager.IsParrying = true;
    }
    public void DisableIsParrying()
    {
        _playerManager.IsParrying = false;
    }
    public void EnableCanBeRiposted()
    {
        _playerManager.CanBeRiposted = true;
    }
    public void DisableCanBeRiposted()
    {
        _playerManager.CanBeRiposted = false;
    }
    public void CanRotate()
    {
        Anim.SetBool("CanRotate", true);
    }
    public void StopRotation()
    {
         Anim.SetBool("CanRotate", false);
    }


    public override void TakeCriticalDamageAnimationEvent()
    {
        _playerStatsManager.TakeDamageNoAnimation(_playerManager.PendingCriticalDamage);
        _playerManager.PendingCriticalDamage = 0;
    } 

    #endregion

}
