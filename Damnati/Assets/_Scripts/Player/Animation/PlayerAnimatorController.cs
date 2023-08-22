using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : AnimatorManager
{
    private InputHandler _inputHandler;
    private PlayerStats _playerStats;
    private PlayerLocomotion _playerLocomotion;
    private PlayerManager _playerManager;

    private int _horizontalVelocity;
    private int _verticalVelocity;


    private bool _hasAnimator;

    #region  GET & SET
    public bool HasAnimator { get { return _hasAnimator; } set { _hasAnimator = value; }}
    
    #endregion

    private void Awake() 
    {
        _hasAnimator = TryGetComponent<Animator>(out Anim);

        Anim = GetComponent<Animator>(); 

        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _playerStats = GetComponent<PlayerStats>();
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
    public void CanRotate()
    {
        Anim.SetBool("CanRotate", true);
    }
    public void StopRotation()
    {
         Anim.SetBool("CanRotate", false);
    }

    private void OnAnimationMove()
    {
        if(!HasAnimator)
        {
            return;
        }
        if(_playerManager.IsInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        _playerLocomotion.PlayerRB.drag = 0;
        Vector3 deltaPos = Anim.deltaPosition;
        deltaPos.y = 0;
        Vector3 velocity = deltaPos / delta;
        _playerLocomotion.PlayerRB.velocity = velocity;
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

    public override void TakeCriticalDamageAnimationEvent()
    {
        _playerStats.TakeDamageNoAnimation(_playerManager.PendingCriticalDamage);
        _playerManager.PendingCriticalDamage = 0;
    } 

    #endregion

}
