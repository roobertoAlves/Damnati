using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : States
{
    [SerializeField] private PursueTargetState _persueTarget;
    [SerializeField] private LayerMask _detectionLayer;
    public override States Tick(EnemyManager enemy)
    {
        #region Handle Enemy Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.DetectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

            if(characterStats != null)
            {
                if(characterStats.TeamIDNumber != enemy.EnemyStatsManager.TeamIDNumber)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > enemy.MinimumDetectionAngle && viewableAngle < enemy.MaximumDetectionAngle)
                    {
                        if(!enemy.IsDead)
                        {
                            enemy.CurrentTarget = characterStats;
                        }
                    }
                }
            }
        }

        #endregion


        #region  Handle Switching To Next Statate
        if(enemy.CurrentTarget != null)
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
