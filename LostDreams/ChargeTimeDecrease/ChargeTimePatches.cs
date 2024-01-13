using Framework.Managers;
using Gameplay.GameControllers.AnimationBehaviours.Player.Attack;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using UnityEngine;

namespace LostDreams.ChargeTimeDecrease;

[HarmonyPatch(typeof(ChargedAttack), "SetChargingTimeByTier")]
class ChargedAttack_SetTimer_Patch
{
    public static void Postfix(ref float ____currentChargingTime)
    {
        // tier 1 = 1.5, tier 2 = 0.75
        if (ChargeTimeEffect.Active)
        {
            Main.LostDreams.Log("RB501: Reducing charge time");
            ____currentChargingTime = 0.05f;
        }
    }
}

[HarmonyPatch(typeof(ChargedAttackBehaviour), "OnStateEnter")]
class ChargedAnimation_Enter_Patch
{
    public static void Postfix(Animator animator)
    {
        if (ChargeTimeEffect.Active)
        {
            animator.speed = 4;
        }
    }
}
[HarmonyPatch(typeof(ChargedAttackBehaviour), "OnStateExit")]
class ChargedAnimation_Exit_Patch
{
    public static void Postfix(Animator animator)
    {
        if (ChargeTimeEffect.Active)
        {
            animator.speed = 1;
        }
    }
}

/// <summary>
/// When petting the dog, set a flag for the level editor
/// </summary>
[HarmonyPatch(typeof(InputManager), nameof(InputManager.SetBlocker))]
class Input_DogBlock_Patch
{
    public static void Postfix(string name)
    {
        if (name == "dog_block")
            Core.Events.SetFlag("PET_DOG", true);
    }
}
