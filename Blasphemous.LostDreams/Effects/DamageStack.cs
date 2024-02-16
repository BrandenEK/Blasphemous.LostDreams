using Framework.Managers;
using Gameplay.GameControllers.Enemies.Framework.Damage;
using Gameplay.GameControllers.Entities;
using Gameplay.GameControllers.Penitent.Damage;
using HarmonyLib;
using System;

namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// Reset when getting hit, increase charges when killing
/// </summary>
internal class DamageStack : IMultiplierEffect
{
    private int _charges;

    public float Multiplier => 1 + (MAX_MULTIPLIER - 1) * ((float)_charges / MAX_CHARGES);

    public DamageStack()
    {
        Main.LostDreams.EventHandler.OnPlayerDamaged += ResetCharges;
        Main.LostDreams.EventHandler.OnEnemyKilled += IncreaseCharges;
        Main.LostDreams.EventHandler.OnExitGame += ResetCharges;
    }

    private void IncreaseCharges()
    {
        if (!Main.LostDreams.ItemHandler.IsEquipped("RB502"))
            return;

        _charges = Math.Min(_charges + 1, MAX_CHARGES);
        Main.LostDreams.Log($"RB502: Increasing damage stack to {_charges}");
    }

    private void ResetCharges()
    {
        _charges = 0;
        Main.LostDreams.Log("RB502: Resetting damage stack");
    }

    private const int MAX_CHARGES = 20;
    private const float MAX_MULTIPLIER = 2.0f;
}

[HarmonyPatch(typeof(PenitentDamageArea), nameof(PenitentDamageArea.TakeDamage))]
class Penitent_DamageStack_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Prefix(ref Hit hit)
    {
        if (Main.LostDreams.ItemHandler.IsEquipped("RB502") && !Core.Logic.Penitent.Status.Unattacable)
        {
            hit.DamageAmount *= Main.LostDreams.DamageStack.Multiplier;
        }
    }
}

[HarmonyPatch(typeof(EnemyDamageArea), nameof(EnemyDamageArea.TakeDamage))]
class Enemy_Damage_Patch
{
    [HarmonyPriority(Priority.High)]
    public static void Prefix(ref Hit hit)
    {
        if (Main.LostDreams.ItemHandler.IsEquipped("RB502"))
        {
            hit.DamageAmount *= Main.LostDreams.DamageStack.Multiplier;
        }
    }
}
