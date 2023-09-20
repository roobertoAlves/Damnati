using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
public class HeavyAttackAction : ItemActions
{
    public override void PerformAction(PlayerManager player)
    {
        player.PlayerAnimator.EraseHandIKForWeapon();
        player.PlayerEffects.PlayWeaponFX(false);

        if(player.IsSprinting)
        {
            HandleJumpingAttack(player);
            return;
        }

        if(player.CanDoCombo)
        {
            player.PlayerInput.ComboFlag = true;
            HandleHeavyWeaponCombo(player);
            player.PlayerInput.ComboFlag = false;
        }

        else
        {
            if(player.IsInteracting || player.CanDoCombo)
            {
                return;
            }

            HandleHeavyAttack(player);
        }
    }
    private void HandleHeavyAttack(PlayerManager player)
    {
        if(player.IsUsingLeftHand)
        {
            player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_01, true, false, true);
            player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_01;
        }
        else if(player.IsUsingRightHand)
        {
            if(player.PlayerInput.TwoHandFlag)
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Heavy_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Heavy_Attack_01;
            }
            else
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_01;
            }
        }
    }
    private void HandleJumpingAttack(PlayerManager player)
    {
        if(player.IsUsingLeftHand)
        {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Jumping_Attack_01, true, false, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Jumping_Attack_01;  
        }
        else if(player.IsUsingRightHand)
        {
            if(player.PlayerInput.TwoHandFlag)
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Jumping_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Jumping_Attack_01;
            }
            else
            {
                player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Jumping_Attack_01, true);
                player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Jumping_Attack_01;
            }
        }
    }
    private void HandleHeavyWeaponCombo(PlayerManager player)
    {
        if(player.Animator.GetBool("IsInteracting") == true && player.Animator.GetBool("CanDoCombo") == false)
        {
            return;
        }

        if(player.PlayerInput.ComboFlag)
        {
            player.Animator.SetBool("CanDoCombo", false);
            
            if(player.IsUsingLeftHand)
            {
                if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Heavy_Attack_01)
                {
                    player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_02, true, false, true);
                    player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_02;
                }
                else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Heavy_Attack_02)
                {
                    player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_03, true, false, true);
                    player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_03;
                }
                else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Heavy_Attack_03)
                {
                    player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_01, true, false, true);
                    player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_01;
                }
            }
            else if(player.IsUsingRightHand)
            {
                if(player.IsTwoHandingWeapon)
                {
                    if(player.PlayerCombat.LastAttack == player.PlayerCombat.TH_Heavy_Attack_01)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Heavy_Attack_02, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Heavy_Attack_02;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.TH_Heavy_Attack_02)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Heavy_Attack_03, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Heavy_Attack_03;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.TH_Heavy_Attack_03)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.TH_Heavy_Attack_01, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.TH_Heavy_Attack_01;
                    }
                }
                else
                {
                    if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Heavy_Attack_01)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_02, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_02;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Heavy_Attack_02)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_03, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_03;
                    }
                    else if(player.PlayerCombat.LastAttack == player.PlayerCombat.OH_Heavy_Attack_03)
                    {
                        player.PlayerAnimator.PlayTargetAnimation(player.PlayerCombat.OH_Heavy_Attack_01, true);
                        player.PlayerCombat.LastAttack = player.PlayerCombat.OH_Heavy_Attack_01;
                    }
                }
            }
        }
    }
}
