using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class IdleStateHumanoid : States
{
    private PursueTargetStateHumanoid _persueTarget;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private LayerMask _layersThatBlockLineOffSight;

    private void Awake() 
    {
        _persueTarget = GetComponent<PursueTargetStateHumanoid>();
    }
    
    public override States Tick(AICharacterManager aiCharacterManager)
    {
        #region Handle AI Character Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacterManager.DetectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter != null)
            {
                if(targetCharacter.CharacterStats.TeamIDNumber != aiCharacterManager.CharacterStats.TeamIDNumber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if(viewableAngle > aiCharacterManager.MinimumDetectionAngle && viewableAngle < aiCharacterManager.MaximumDetectionAngle)
                    {
                        if(!aiCharacterManager.IsDead)
                        {
                            if(Physics.Linecast(aiCharacterManager.LockOnTransform.position, targetCharacter.LockOnTransform.position, _layersThatBlockLineOffSight))
                            {
                                return this;
                            }
                            else
                            {
                                aiCharacterManager.CurrentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region  Handle Switching To Next Statate
        if(aiCharacterManager.CurrentTarget != null)
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
