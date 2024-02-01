using Blasphemous.ModdingAPI.Items;
using System;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.DamageStack;

class DamageStackBead : ModRosaryBead
{
    protected override string Id => "RB502";

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize("dsname");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize("dsdesc");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize("dslore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite("damage-stack.png", out picture);
    }
}

class DamageStackEffect : ModItemEffectOnEquip
{
    private static int _charges;

    public static float CurrentMultiplier => 1 + (MAX_MULTIPLIER - 1) * ((float)_charges / MAX_CHARGES);

    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("damage-stack");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("damage-stack");

    public DamageStackEffect()
    {
        Main.LostDreams.EventHandler.OnPlayerDamaged += ResetCharges;
        Main.LostDreams.EventHandler.OnEnemyKilled += IncreaseCharges;
        Main.LostDreams.EventHandler.OnExitGame += ResetCharges;
    }

    private static void IncreaseCharges()
    {
        if (!Main.LostDreams.EffectHandler.IsActive("damage-stack"))
            return;

        Main.LostDreams.Log($"RB502: Increasing damage stack to {_charges + 1}");
        _charges = Math.Min(_charges + 1, MAX_CHARGES);
    }

    private static void ResetCharges()
    {
        Main.LostDreams.Log("RB502: Resetting damage stack");
        _charges = 0;
    }

    private const int MAX_CHARGES = 20;
    private const float MAX_MULTIPLIER = 2.0f;
}
