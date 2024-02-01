using Blasphemous.ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.ChargeTime;

class ChargeTimeBead : ModRosaryBead
{
    protected override string Id => "RB501";

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize("ctname");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize("ctdesc");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize("ctlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite("charge-time.png", out picture);
    }
}

class ChargeTimeEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("charge-time");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("charge-time");
}
