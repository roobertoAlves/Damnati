using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : States
{
    [SerializeField] private PursueTargetState _persueTarget;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private LayerMask _layersThatBlockLineOffSight;
    public override States Tick(AICharacterManager aICharacterManager)
    {
        #region Handle AI Character Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, aICharacterManager.DetectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter != null)
            {
                if(targetCharacter.CharacterStats.TeamIDNumber != aICharacterManager.CharacterStats.TeamIDNumber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > aICharacterManager.MinimumDetectionAngle && viewableAngle < aICharacterManager.MaximumDetectionAngle)
                    {
                        if(!aICharacterManager.IsDead)
                        {
                            if(Physics.Linecast(aICharacterManager.LockOnTransform.position, targetCharacter.LockOnTransform.position, _layersThatBlockLineOffSight))
                            {
                                return this;
                            }
                            else
                            {
                                aICharacterManager.CurrentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region  Handle Switching To Next Statate
        if(aICharacterManager.CurrentTarget != null)
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
