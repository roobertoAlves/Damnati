using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : States
{
    [SerializeField] private PersueTargetState _persueTarget;
    [SerializeField] private LayerMask _detectionLayer;
    public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorController enemyAnimatorController)
    {
        #region Handle Enemy Target Detection
        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.DetectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if(characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position -  enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection,  enemyManager.transform.forward);

                if(viewableAngle > enemyManager.MinimumDetectionAngle && viewableAngle < enemyManager.MaximumDetectionAngle)
                {
                    if(!characterStats.IsDead)
                    {
                        enemyManager.CurrentTarget = characterStats;
                    }
                }
            }
        }

        #endregion


        #region  Handle Switching To Next Statate
        if(enemyManager.CurrentTarget != null)
        {
            return _persueTarget;
        }
        else
        {
            return this;
        }

        #endregion
    }
}
