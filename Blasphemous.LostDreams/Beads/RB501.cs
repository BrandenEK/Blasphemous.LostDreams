using Blasphemous.Framework.Items;
using Gameplay.GameControllers.AnimationBehaviours.Player.Attack;
using Gameplay.GameControllers.Penitent.Abilities;
using HarmonyLib;
using UnityEngine;

namespace Blasphemous.LostDreams.Beads;

internal class RB501 : ModItemEffectOnEquip
{
    protected override void ApplyEffect()
    {
        DecreaseChangeTime = true;
    }

    protected override void RemoveEffect()
    {
        DecreaseChangeTime = false;
    }

    protected override void Update()
    {
        Main.LostDreams.LogWarning("Decrease: " + DecreaseChangeTime);
    }

    public static bool DecreaseChangeTime { get; private set; }
}

/// <summary>
/// Reduce how much time to activate charge
/// </summary>
[HarmonyPatch(typeof(ChargedAttack), "SetChargingTimeByTier")]
class RB501_ChargedAttack_SetTimer_Patch
{
    public static void Postfix(ref float ____currentChargingTime)
    {
        // tier 1 = 1.5, tier 2 = 0.75
        if (Main.LostDreams.ItemHandler.IsEquipped("RB501"))
        {
            Main.LostDreams.Log("RB501: Reducing charge time");
            ____currentChargingTime = 0.05f;
        }
    }
}

/// <summary>
/// Increase speed of charging animation
/// </summary>
[HarmonyPatch(typeof(ChargedAttackBehaviour), "OnStateEnter")]
class RB501_ChargedAnimation_Enter_Patch
{
    public static void Postfix(Animator animator)
    {
        if (Main.LostDreams.ItemHandler.IsEquipped("RB501"))
        {
            animator.speed = 4;
        }
    }
}
[HarmonyPatch(typeof(ChargedAttackBehaviour), "OnStateExit")]
class RB501_ChargedAnimation_Exit_Patch
{
    public static void Postfix(Animator animator)
    {
        if (Main.LostDreams.ItemHandler.IsEquipped("RB501"))
        {
            animator.speed = 1;
        }
    }
}