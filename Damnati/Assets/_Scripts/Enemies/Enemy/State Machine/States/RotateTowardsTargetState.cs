using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetState : States
{
    [SerializeField] private CombatStanceState _combatStanceState;
    public override States Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.Anim.SetFloat("Vertical", 0);
        enemyAnimatorManager.Anim.SetFloat("Horizontal", 0);

        Vector3 targetDirection = enemyManager.CurrentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

        if(enemyManager.IsInteracting)
        {
            return this; 
        }

        if(viewableAngle >= 100 && viewableAngle <= 180
        && !enemyManager.IsInteracting)
        {
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            Debug.Log("back");
            return _combatStanceState;
        }

        else if(viewableAngle <= -101 && viewableAngle >= -180
        && !enemyManager.IsInteracting)
        {
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Behind", true);
            Debug.Log("back");
            return _combatStanceState;
        }

        else if(viewableAngle <= -45 && viewableAngle >= -100
        && !enemyManager.IsInteracting)
        {
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Right", true);
            Debug.Log("right");
            return _combatStanceState;
        }
        
        else if(viewableAngle >= 45 && viewableAngle <= 100
        && !enemyManager.IsInteracting)
        {
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("Turn Left", true);
            Debug.Log("left");
            return _combatStanceState;
        }
        
        return _combatStanceState;
    }
}
