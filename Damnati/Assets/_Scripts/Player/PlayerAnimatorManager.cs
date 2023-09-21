using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    private PlayerManager _player;
    private int _horizontalVelocity;
    private int _verticalVelocity;



    protected override void Awake() 
    {

        base.Awake();
        _player = GetComponent<PlayerManager>();

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

        if (isSprinting && _player.PlayerInput.MoveAmount > 0)
        {
            v = 2;
            h = horizontalMovement;
        }

        _player.Animator.SetFloat(_verticalVelocity, v, 0.1f, Time.deltaTime);
        _player.Animator.SetFloat(_horizontalVelocity, h, 0.1f, Time.deltaTime);
    }
   
    public void DisableCollision()
    {
        _player.PlayerLocomotion.CharacterCollider.enabled = false;
        _player.PlayerLocomotion.CharacterCollisionBlockerCollider.enabled = false;
    }
    public void EnableCollision()
    {
        _player.PlayerLocomotion.CharacterCollider.enabled = true;
        _player.PlayerLocomotion.CharacterCollisionBlockerCollider.enabled = true;

    }
    private void OnAnimationMove()
    {
        if(_player.IsInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        _player.PlayerLocomotion.PlayerRB.drag = 0;
        Vector3 deltaPos = _player.Animator.deltaPosition;
        deltaPos.y = 0;
        Vector3 velocity = deltaPos / delta;
        _player.PlayerLocomotion.PlayerRB.velocity = velocity;
    }
}
