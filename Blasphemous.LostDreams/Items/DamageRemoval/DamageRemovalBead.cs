using ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.DamageRemoval;

class DamageRemovalBead : ModRosaryBead
{
    protected override string Id => "RB503";

    protected override string Name => Main.Blasphemous.LostDreams.Localize("drname");

    protected override string Description => Main.Blasphemous.LostDreams.Localize("drdesc");

    protected override string Lore => Main.Blasphemous.LostDreams.Localize("drlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.Blasphemous.LostDreams.FileUtil.loadDataImages("damage-removal.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
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
        Main.Blasphemous.LostDreams.EventHandler.OnPlayerKilled += RegainDamageRemoval;
        Main.Blasphemous.LostDreams.EventHandler.OnUsePrieDieu += RegainDamageRemoval;
        Main.Blasphemous.LostDreams.EventHandler.OnExitGame += RegainDamageRemoval;
        Main.Blasphemous.LostDreams.EventHandler.OnExitGame += UnequipBead;
        Main.Blasphemous.LostDreams.EventHandler.OnPlayerDamaged += UseDamageRemoval;
    }

    private void CheckForActivation()
    {
        if (_equipped && !_alreadyUsed)
            Main.Blasphemous.LostDreams.EffectHandler.Activate("damage-removal");
        else
            Main.Blasphemous.LostDreams.EffectHandler.Deactivate("damage-removal");
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
