using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : States
{
    [SerializeField] private PursueTargetState _persueTarget;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private LayerMask _layersThatBlockLineOffSight;
    public override States Tick(EnemyManager aiCharacter)
    {
        #region Handle AI Character Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.DetectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter != null)
            {
                if(targetCharacter.CharacterStats.TeamIDNumber != aiCharacter.CharacterStats.TeamIDNumber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > aiCharacter.MinimumDetectionAngle && viewableAngle < aiCharacter.MaximumDetectionAngle)
                    {
                        if(!aiCharacter.IsDead)
                        {
                            if(Physics.Linecast(aiCharacter.LockOnTransform.position, targetCharacter.LockOnTransform.position, _layersThatBlockLineOffSight))
                            {
                                return this;
                            }
                            else
                            {
                                aiCharacter.CurrentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region  Handle Switching To Next Statate
        if(aiCharacter.CurrentTarget != null)
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
