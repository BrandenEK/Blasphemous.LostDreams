using ModdingAPI.Items;
using System;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.DamageStack;

class DamageStackBead : ModRosaryBead
{
    protected override string Id => "RB502";

    protected override string Name => Main.Blasphemous.LostDreams.Localize("dsname");

    protected override string Description => Main.Blasphemous.LostDreams.Localize("dsdesc");

    protected override string Lore => Main.Blasphemous.LostDreams.Localize("dslore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.Blasphemous.LostDreams.FileUtil.loadDataImages("damage-stack.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class DamageStackEffect : ModItemEffectOnEquip
{
    private static int _charges;

    public static float CurrentMultiplier => 1 + (MAX_MULTIPLIER - 1) * ((float)_charges / MAX_CHARGES);

    protected override void ApplyEffect() => Main.Blasphemous.LostDreams.EffectHandler.Activate("damage-stack");

    protected override void RemoveEffect() => Main.Blasphemous.LostDreams.EffectHandler.Deactivate("damage-stack");

    public DamageStackEffect()
    {
        Main.Blasphemous.LostDreams.EventHandler.OnPlayerDamaged += ResetCharges;
        Main.Blasphemous.LostDreams.EventHandler.OnEnemyKilled += IncreaseCharges;
        Main.Blasphemous.LostDreams.EventHandler.OnExitGame += ResetCharges;
    }

    private static void IncreaseCharges()
    {
        if (!Main.Blasphemous.LostDreams.EffectHandler.IsActive("damage-stack"))
            return;

        Main.Blasphemous.LostDreams.Log($"RB502: Increasing damage stack to {_charges + 1}");
        _charges = Math.Min(_charges + 1, MAX_CHARGES);
    }

    private static void ResetCharges()
    {
        Main.Blasphemous.LostDreams.Log("RB502: Resetting damage stack");
        _charges = 0;
    }

    private const int MAX_CHARGES = 20;
    private const float MAX_MULTIPLIER = 2.0f;
}
