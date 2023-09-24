using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
public class HeavyAttackAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
        character.CharacterAnimator.EraseHandIKForWeapon();
        character.CharacterEffects.PlayWeaponFX(false);

        if(character.IsSprinting)
        {
            HandleJumpingAttack(character);
            character.CharacterCombat.CurrentAttackType = AttackType.JumpingHeavyAttack;
            return;
        }

        if(character.CanDoCombo)
        {
            HandleHeavyWeaponCombo(character);
            character.CharacterCombat.CurrentAttackType = AttackType.HeavyAttack;
            character.CanDoCombo = false;
        }

        else
        {
            if(character.IsInteracting || character.CanDoCombo)
            {
                return;
            }

            HandleHeavyAttack(character);
            character.CharacterCombat.CurrentAttackType = AttackType.HeavyAttack;
        }
    }
    private void HandleHeavyAttack(CharacterManager character)
    {
        if(character.IsUsingLeftHand)
        {
            character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_01, true, false, true);
            character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_01;
        }
        else if(character.IsUsingRightHand)
        {
            if(character.IsTwoHandingWeapon)
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Heavy_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Heavy_Attack_01;
            }
            else
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_01;
            }
        }
    }
    private void HandleJumpingAttack(CharacterManager character)
    {
        if(character.IsUsingLeftHand)
        {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Jumping_Attack_01, true, false, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Jumping_Attack_01;  
        }
        else if(character.IsUsingRightHand)
        {
            if(character.IsTwoHandingWeapon)
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Jumping_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Jumping_Attack_01;
            }
            else
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Jumping_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Jumping_Attack_01;
            }
        }
    }
    private void HandleHeavyWeaponCombo(CharacterManager character)
    {
        if(character.Animator.GetBool("IsInteracting") == true && character.Animator.GetBool("CanDoCombo") == false)
        {
            return;
        }

        if(character.CanDoCombo)
        {
            character.Animator.SetBool("CanDoCombo", false);
            
            if(character.IsUsingLeftHand)
            {
                if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Heavy_Attack_01)
                {
                    character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_02, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_02;
                }
                else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Heavy_Attack_02)
                {
                    character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_03, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_03;
                }
                else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Heavy_Attack_03)
                {
                    character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_01, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_01;
                }
            }
            else if(character.IsUsingRightHand)
            {
                if(character.IsTwoHandingWeapon)
                {
                    if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Heavy_Attack_01)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Heavy_Attack_02, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Heavy_Attack_02;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Heavy_Attack_02)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Heavy_Attack_03, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Heavy_Attack_03;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Heavy_Attack_03)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Heavy_Attack_01, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Heavy_Attack_01;
                    }
                }
                else
                {
                    if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Heavy_Attack_01)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_02, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_02;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Heavy_Attack_02)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_03, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_03;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Heavy_Attack_03)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Heavy_Attack_01, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Heavy_Attack_01;
                    }
                }
            }
        }
    }
}
