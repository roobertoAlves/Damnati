using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateHumanoid : States
{
    [Header("AI Scripts Components")]
    [Space(15)]
    private CombatStanceStateHumanoid combatStanceState;
    private PursueTargetStateHumanoid pursueTargetState;
    private RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
    private IdleStateHumanoid _idleState;
    private ItemBasedAttackAction currentAttack;

    private bool willDoComboOnNextAttack = false;
    [SerializeField] private bool hasPerformedAttack = false;

    #region GET & SET
    public ItemBasedAttackAction CurrentAttack { get { return currentAttack; } set { currentAttack = value; }}
    public bool HasPerformedAttack { get { return hasPerformedAttack; } set { hasPerformedAttack = value; }}
    #endregion

    private void Awake()
    {
        _idleState = GetComponent<IdleStateHumanoid>();
        combatStanceState = GetComponent<CombatStanceStateHumanoid>();
        rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
        pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
    }

    public override States Tick(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager.CombatStyle == AICombatStyle.SwordAndShield)
        {
            return ProcessSwordAndShieldCombatStyle(aiCharacterManager);
        }
        else if (aiCharacterManager.CombatStyle == AICombatStyle.Archer)
        {
            return ProcessArcherCombatStyle(aiCharacterManager);
        }
        else
        {
            return this;
        }
    }

    private States ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacterManager)
    {   
        RotateTowardsTargetWhilstAttacking(aiCharacterManager);

        if (aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            ResetStatesFlags();
            return pursueTargetState;
        }

        if (willDoComboOnNextAttack && aiCharacterManager.CanDoCombo)
        {
            AttackTargetWithCombo(aiCharacterManager);
        }

        if (!hasPerformedAttack)
        {
            AttackTarget(aiCharacterManager);
            RollForComboChance(aiCharacterManager);
        }
        if (aiCharacterManager.CurrentTarget.IsDead)
        {
            ResetStatesFlags();
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            aiCharacterManager.CurrentTarget = null;
            return _idleState;
        }

        if (willDoComboOnNextAttack && hasPerformedAttack)
        {
            ResetStatesFlags();
            return this;
        }

        ResetStatesFlags(); 
        return rotateTowardsTargetState;
    }

    private States ProcessArcherCombatStyle(AICharacterManager aiCharacterManager)
    {
        RotateTowardsTargetWhilstAttacking(aiCharacterManager);

        if (aiCharacterManager.IsInteracting)
        {
            ResetStatesFlags(); // Adicione essa linha para redefinir as flags quando estiver interagindo
            return this;
        }

        if (!aiCharacterManager.IsHoldingArrow)
        {
            ResetStatesFlags();
            return combatStanceState;
        }

        if (aiCharacterManager.CurrentTarget.IsDead)
        {
            ResetStatesFlags();
            aiCharacterManager.Animator.SetFloat("Vertical", 0);
            aiCharacterManager.Animator.SetFloat("Horizontal", 0);
            aiCharacterManager.CurrentTarget = null;
            return _idleState;
        }

        if (aiCharacterManager.DistanceFromTarget > aiCharacterManager.MaximumAggroRadius)
        {
            ResetStatesFlags();
            return pursueTargetState;
        }

        if (!hasPerformedAttack)
        {
            FireAmmo(aiCharacterManager);
        }  

        ResetStatesFlags();
        return rotateTowardsTargetState;
    }

    private void AttackTarget(AICharacterManager aiCharacterManager)
    {
        currentAttack.PerformAttackAction(aiCharacterManager);
        aiCharacterManager.CurrentRecoveryTime = currentAttack.RecoveryTime;
        hasPerformedAttack = true;
    }

    private void AttackTargetWithCombo(AICharacterManager aiCharacterManager)
    {
        currentAttack.PerformAttackAction(aiCharacterManager);
        willDoComboOnNextAttack = false;
        aiCharacterManager.CurrentRecoveryTime = currentAttack.RecoveryTime;
        currentAttack = null;
    }

    private void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager.CanRotate && aiCharacterManager.IsInteracting)
        {
            Vector3 direction = aiCharacterManager.CurrentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacterManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacterManager.RotationSpeed / Time.deltaTime);
        }
    }

    private void RollForComboChance(AICharacterManager aiCharacterManager)
    {
        float comboChance = Random.Range(0, 100);

        if (aiCharacterManager.AllowAIToPerformCombos && comboChance <= aiCharacterManager.ComboLikelyHood)
        {
            if (currentAttack.ActionCanCombo)
            {
                willDoComboOnNextAttack = true;
            }
            else
            {
                willDoComboOnNextAttack = false;
                currentAttack = null;
            }
        }
    }

    private void FireAmmo(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager.IsHoldingArrow)
        {
            hasPerformedAttack = true;
            aiCharacterManager.CharacterInventory.CurrentItemBeingUsed = aiCharacterManager.CharacterInventory.rightHandWeapon;
            aiCharacterManager.CharacterInventory.rightHandWeapon.th_release_RB_Action.PerformAction(aiCharacterManager);
        }
    }

    private void ResetStatesFlags()
    {
        willDoComboOnNextAttack = false;
        hasPerformedAttack = false;   
    }
}