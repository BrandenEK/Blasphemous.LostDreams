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
    private readonly int _maxCharges;
    private readonly float _maxMultiplier;

    private int _currentCharges;

    public float Multiplier => 1 + (_maxMultiplier - 1) * ((float)_currentCharges / _maxCharges);

    public DamageStack(int maxCharges, float maxMultiplier)
    {
        _maxCharges = maxCharges;
        _maxMultiplier = maxMultiplier;

        Main.LostDreams.EventHandler.OnPlayerDamaged += ResetCharges;
        Main.LostDreams.EventHandler.OnEnemyKilled += IncreaseCharges;
        Main.LostDreams.EventHandler.OnExitGame += ResetCharges;
    }

    private void IncreaseCharges()
    {
        if (!Main.LostDreams.ItemHandler.IsEquipped("RB502"))
            return;

        _currentCharges = Math.Min(_currentCharges + 1, _maxCharges);
        Main.LostDreams.Log($"RB502: Increasing damage stack to {_currentCharges}");
    }

    private void ResetCharges()
    {
        _currentCharges = 0;
        Main.LostDreams.Log("RB502: Resetting damage stack");
    }
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
