using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatStanceState : CombatStanceState
{
    [Header("Second Phase Attacks")]
    [Space(15)]
    [SerializeField] private bool _hasPhaseShifted;
    [SerializeField] private EnemyAttackAction[] _secondPhaseAttacks;

    #region GET & SET
    public bool HasPhaseShifted { get { return _hasPhaseShifted; } set { _hasPhaseShifted = value; }}
    public EnemyAttackAction[] SecondPhaseAttacks { get { return _secondPhaseAttacks;}}
    #endregion
    protected override void GetNewAttack(EnemyManager enemy)
    {
        if(_hasPhaseShifted)
        {
            Vector3 targetsDirection = enemy.CurrentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemy.CurrentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < _secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = _secondPhaseAttacks[i];

                if(distanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
                {
                    if(viewableAngle <= enemyAttackAction.MaximumAttackAngle
                        && viewableAngle >= enemyAttackAction.MinimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.AttackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < _secondPhaseAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = _secondPhaseAttacks[i];

                if(distanceFromTarget <= enemyAttackAction.MaximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.MinimumDistanceNeededToAttack)
                {
                    if(viewableAngle <= enemyAttackAction.MaximumAttackAngle
                    && viewableAngle >= enemyAttackAction.MinimumAttackAngle)
                    {
                       if(AttackingState.CurrentAttack != null)
                        {
                            return;
                        }

                        temporaryScore += enemyAttackAction.AttackScore;

                        if(temporaryScore > randomValue)
                        {
                            AttackingState.CurrentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
        else
        {
            base.GetNewAttack(enemy);
        }
    }
}
