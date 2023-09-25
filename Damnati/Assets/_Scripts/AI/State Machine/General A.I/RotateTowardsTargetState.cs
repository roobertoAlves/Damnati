using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;
    public override States Tick(AICharacterManager aICharacterManager)
    {
        aICharacterManager.Animator.SetFloat("Vertical", 0);
        aICharacterManager.Animator.SetFloat("Horizontal", 0);

        if(aICharacterManager.IsInteracting)
        {
            return this; 
        }

        if(aICharacterManager.ViewableAngle >= 100 && aICharacterManager.ViewableAngle <= 180
        && !aICharacterManager.IsInteracting)
        {
            aICharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(aICharacterManager.ViewableAngle <= -101 && aICharacterManager.ViewableAngle >= -180
        && !aICharacterManager.IsInteracting)
        {
            aICharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(aICharacterManager.ViewableAngle <= -45 && aICharacterManager.ViewableAngle >= -100
        && !aICharacterManager.IsInteracting)
        {
            aICharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Right", true);
            //Debug.Log("right");
            return _combatStanceState;
        }
        
        else if(aICharacterManager.ViewableAngle >= 45 && aICharacterManager.ViewableAngle <= 100
        && !aICharacterManager.IsInteracting)
        {
            aICharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Left", true);
            //Debug.Log("left");
            return _combatStanceState;
        }
        
        return _combatStanceState;
    }
}
