using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
public class ChargeAttackAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
       character.CharacterAnimator.EraseHandIKForWeapon();
       character.CharacterEffects.PlayWeaponFX(false);

        if(character.CanDoCombo)
        {
            HandleChargeWeaponCombo(character);
            character.CanDoCombo = false;
        }

        else
        {
            if(character.IsInteracting || character.CanDoCombo)
            {
                return;
            }

            HandleChargeAttack(character);
        }
    }
    private void HandleChargeAttack(CharacterManager character)
    {
        if(character.IsUsingLeftHand)
        {
            character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Charge_Attack_01, true, false, true);
            character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Charge_Attack_01;
        }
        else if(character.IsUsingRightHand)
        {
            if(character.IsTwoHandingWeapon)
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Charge_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Charge_Attack_01;
            }
            else
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Charge_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Charge_Attack_01;
            }
        }
    }
    private void HandleChargeWeaponCombo(CharacterManager character)
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
                if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Charge_Attack_01)
                {
                   character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Charge_Attack_02, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Charge_Attack_02;
                }
                else
                {
                   character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Charge_Attack_01, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Charge_Attack_01;
                }
            }
            else if(character.IsUsingRightHand)
            {
                if(character.IsTwoHandingWeapon)
                {
                    if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Charge_Attack_01)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Charge_Attack_02, true);
                        character.CharacterCombat.LastAttack =character.CharacterCombat.TH_Charge_Attack_02;
                    }
                    else
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Charge_Attack_01, true);
                        character.CharacterCombat.LastAttack =character.CharacterCombat.TH_Charge_Attack_01;
                    }
                }
                else
                {
                    if(character.CharacterCombat.LastAttack ==character.CharacterCombat.OH_Charge_Attack_01)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Charge_Attack_02, true);
                        character.CharacterCombat.LastAttack =character.CharacterCombat.OH_Charge_Attack_02;
                    }
                    else
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Charge_Attack_01, true);
                        character.CharacterCombat.LastAttack =character.CharacterCombat.OH_Charge_Attack_01;
                    }
                }
            }
        }
    }
}
