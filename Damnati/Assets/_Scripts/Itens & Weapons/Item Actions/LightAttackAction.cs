using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Light Attack Actions")]
public class LightAttackAction : ItemActions
{
    public override void PerformAction(CharacterManager character)
    {
        character.IsAttacking = true;
        character.CharacterAnimator.EraseHandIKForWeapon();
        character.CharacterEffects.PlayWeaponFX(false);

        if(character.IsSprinting)
        {
            HandleRunningAttack(character);
            //Debug.Log("correndo");
            character.CharacterCombat.CurrentAttackType = AttackType.RunningLightAttack;
            return;
            
        }

        if(character.CanDoCombo)
        {
            HandleLightWeaponCombo(character);
            character.CharacterCombat.CurrentAttackType = AttackType.LightAttack;
            character.CanDoCombo = false;
        }
        else
        {
            if(character.IsInteracting || character.CanDoCombo)
            {
                return;
            }

            //Debug.Log("ataque");
            HandleLightAttack(character);
            character.CharacterCombat.CurrentAttackType = AttackType.LightAttack;
        }

    }
    private void HandleLightAttack(CharacterManager character)
    {
        if(character.IsUsingLeftHand)
        {
            character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_01, true, false, true);   
            character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_01; 
        }
        else if(character.IsUsingRightHand)
        {
            if(character.IsTwoHandingWeapon)
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Light_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Light_Attack_01;
            }
            else
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_01;
            }
        }
    }
    private void HandleRunningAttack(CharacterManager character)
    {
        if(character.IsUsingLeftHand)
        {
            character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Running_Attack_01, true, false, true);
            character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Running_Attack_01;
        }
        else if(character.IsUsingRightHand)
        {
            if(character.IsTwoHandingWeapon)
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Running_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Running_Attack_01;
            }
            else
            {
                character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Running_Attack_01, true);
                character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Running_Attack_01;
            }
        }
    }
    public void HandleLightWeaponCombo(CharacterManager character)
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
                if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Light_Attack_01)
                {
                    character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_02, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_02;
                }
                else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Light_Attack_02)
                {
                    character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_03, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_03;
                }
                else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Light_Attack_03)
                {
                    character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_01, true, false, true);
                    character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_01;
                }
            }
            else if(character.IsUsingRightHand)
            {
                if(character.IsTwoHandingWeapon)
                {
                    if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Light_Attack_01)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Light_Attack_02, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Light_Attack_02;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Light_Attack_02)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Light_Attack_03, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Light_Attack_03;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.TH_Light_Attack_03)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.TH_Light_Attack_01, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.TH_Light_Attack_01;
                    }
                }
                else
                {
                    if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Light_Attack_01)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_02, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_02;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Light_Attack_02)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_03, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_03;
                    }
                    else if(character.CharacterCombat.LastAttack == character.CharacterCombat.OH_Light_Attack_03)
                    {
                        character.CharacterAnimator.PlayTargetAnimation(character.CharacterCombat.OH_Light_Attack_01, true);
                        character.CharacterCombat.LastAttack = character.CharacterCombat.OH_Light_Attack_01;
                    }
                }
            }
        }
    }
}
