using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatStanceState : CombatStanceState
{
    [Header("Second Phase Attacks")]
    [Space(15)]
    [SerializeField] private bool _hasPhaseShifted;
    [SerializeField] private AICharacterAttackAction[] _secondPhaseAttacks;

    #region GET & SET
    public bool HasPhaseShifted { get { return _hasPhaseShifted; } set { _hasPhaseShifted = value; }}
    public AICharacterAttackAction[] SecondPhaseAttacks { get { return _secondPhaseAttacks;}}
    #endregion
    protected override void GetNewAttack(AICharacterManager aICharacterManager)
    {
        if(_hasPhaseShifted)
        {
            Vector3 targetsDirection = aICharacterManager.CurrentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(aICharacterManager.CurrentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < _secondPhaseAttacks.Length; i++)
            {
                AICharacterAttackAction aICharacterAttackAction = _secondPhaseAttacks[i];

                if(distanceFromTarget <= aICharacterAttackAction.MaximumDistanceNeededToAttack
                    && distanceFromTarget >= aICharacterAttackAction.MinimumDistanceNeededToAttack)
                {
                    if(viewableAngle <= aICharacterAttackAction.MaximumAttackAngle
                        && viewableAngle >= aICharacterAttackAction.MinimumAttackAngle)
                    {
                        maxScore += aICharacterAttackAction.AttackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < _secondPhaseAttacks.Length; i++)
            {
                AICharacterAttackAction aICharacterAttackAction = _secondPhaseAttacks[i];

                if(distanceFromTarget <= aICharacterAttackAction.MaximumDistanceNeededToAttack
                && distanceFromTarget >= aICharacterAttackAction.MinimumDistanceNeededToAttack)
                {
                    if(viewableAngle <= aICharacterAttackAction.MaximumAttackAngle
                    && viewableAngle >= aICharacterAttackAction.MinimumAttackAngle)
                    {
                       if(AttackingState.CurrentAttack != null)
                        {
                            return;
                        }

                        temporaryScore += aICharacterAttackAction.AttackScore;

                        if(temporaryScore > randomValue)
                        {
                            AttackingState.CurrentAttack = aICharacterAttackAction;
                        }
                    }
                }
            }
        }
        else
        {
            base.GetNewAttack(aICharacterManager);
        }
    }
}
