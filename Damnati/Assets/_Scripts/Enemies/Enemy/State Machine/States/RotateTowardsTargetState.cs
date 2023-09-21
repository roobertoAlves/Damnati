using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;
    public override States Tick(EnemyManager enemy)
    {
        enemy.Animator.SetFloat("Vertical", 0);
        enemy.Animator.SetFloat("Horizontal", 0);

        if(enemy.IsInteracting)
        {
            return this; 
        }

        if(enemy.ViewableAngle >= 100 && enemy.ViewableAngle <= 180
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(enemy.ViewableAngle <= -101 && enemy.ViewableAngle >= -180
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(enemy.ViewableAngle <= -45 && enemy.ViewableAngle >= -100
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Right", true);
            //Debug.Log("right");
            return _combatStanceState;
        }
        
        else if(enemy.ViewableAngle >= 45 && enemy.ViewableAngle <= 100
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Left", true);
            //Debug.Log("left");
            return _combatStanceState;
        }
        
        return _combatStanceState;
    }
}
