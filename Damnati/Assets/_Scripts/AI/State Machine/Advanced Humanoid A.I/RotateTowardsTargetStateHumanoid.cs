using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetStateHumanoid : States
{
    private CombatStanceStateHumanoid _combatStanceState;

    private void Awake() 
    {
        _combatStanceState = GetComponent<CombatStanceStateHumanoid>();
    }
    
    public override States Tick(AICharacterManager aiCharacterManager)
    {
        aiCharacterManager.Animator.SetFloat("Vertical", 0);
        aiCharacterManager.Animator.SetFloat("Horizontal", 0);

        if(aiCharacterManager.IsInteracting)
        {
            return this; 
        }

        if(aiCharacterManager.ViewableAngle >= 100 && aiCharacterManager.ViewableAngle <= 180
        && !aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(aiCharacterManager.ViewableAngle <= -101 && aiCharacterManager.ViewableAngle >= -180
        && !aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(aiCharacterManager.ViewableAngle <= -45 && aiCharacterManager.ViewableAngle >= -100
        && !aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Right", true);
            //Debug.Log("right");
            return _combatStanceState;
        }
        
        else if(aiCharacterManager.ViewableAngle >= 45 && aiCharacterManager.ViewableAngle <= 100
        && !aiCharacterManager.IsInteracting)
        {
            aiCharacterManager.AICharacterAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Left", true);
            //Debug.Log("left");
            return _combatStanceState;
        }
        
        return _combatStanceState;
    }
}
