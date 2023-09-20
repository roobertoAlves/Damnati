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

        Vector3 targetDirection = enemy.CurrentTarget.transform.position - enemy.transform.position;
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

        if(enemy.IsInteracting)
        {
            return this; 
        }

        if(viewableAngle >= 100 && viewableAngle <= 180
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(viewableAngle <= -101 && viewableAngle >= -180
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            //Debug.Log("back");
            return _combatStanceState;
        }

        else if(viewableAngle <= -45 && viewableAngle >= -100
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Right", true);
            //Debug.Log("right");
            return _combatStanceState;
        }
        
        else if(viewableAngle >= 45 && viewableAngle <= 100
        && !enemy.IsInteracting)
        {
            enemy.EnemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Left", true);
            //Debug.Log("left");
            return _combatStanceState;
        }
        
        return _combatStanceState;
    }
}
