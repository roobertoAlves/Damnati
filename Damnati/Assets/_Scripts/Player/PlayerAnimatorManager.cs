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
    
}
