using Blasphemous.ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.DamageRemoval;

class DamageRemovalBead : ModRosaryBead
{
    protected override string Id => "RB503";

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize("drname");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize("drdesc");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize("drlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite("damage-removal.png", out picture);
    }
}

class DamageRemovalEffect : ModItemEffectOnEquip
{
    private bool _equipped;
    private bool _alreadyUsed;

    protected override void ApplyEffect() => EquipBead();

    protected override void RemoveEffect() => UnequipBead();

    public DamageRemovalEffect()
    {
        Main.LostDreams.EventHandler.OnPlayerKilled += RegainDamageRemoval;
        Main.LostDreams.EventHandler.OnUsePrieDieu += RegainDamageRemoval;
        Main.LostDreams.EventHandler.OnExitGame += RegainDamageRemoval;
        Main.LostDreams.EventHandler.OnExitGame += UnequipBead;
        Main.LostDreams.EventHandler.OnPlayerDamaged += UseDamageRemoval;
    }

    private void CheckForActivation()
    {
        if (_equipped && !_alreadyUsed)
            Main.LostDreams.EffectHandler.Activate("damage-removal");
        else
            Main.LostDreams.EffectHandler.Deactivate("damage-removal");
    }

    private void EquipBead()
    {
        _equipped = true;
        CheckForActivation();
    }

    private void UnequipBead()
    {
        _equipped = false;
        CheckForActivation();
    }

    private void RegainDamageRemoval()
    {
        _alreadyUsed = false;
        CheckForActivation();
    }

    private void UseDamageRemoval()
    {
        _alreadyUsed = true;
        CheckForActivation();
    }
}
