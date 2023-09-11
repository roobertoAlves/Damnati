using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : States
{
    [SerializeField] private PursueTargetState _persueTarget;
    [SerializeField] private LayerMask _detectionLayer;
    public override States Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        #region Handle Enemy Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.DetectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if(characterStats != null)
            {
                if(characterStats.TeamIDNumber != enemyStats.TeamIDNumber)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > enemyManager.MinimumDetectionAngle && viewableAngle < enemyManager.MaximumDetectionAngle)
                    {
                        if(!characterStats.IsDead)
                        {
                            enemyManager.CurrentTarget = characterStats;
                        }
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
