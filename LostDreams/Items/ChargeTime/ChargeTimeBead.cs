using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.Items.ChargeTime;

class ChargeTimeBead : ModRosaryBead
{
    protected override string Id => "RB501";

    protected override string Name => Main.LostDreams.Localize("ctname");

    protected override string Description => Main.LostDreams.Localize("ctdesc");

    protected override string Lore => Main.LostDreams.Localize("ctlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("charge-time.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class ChargeTimeEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("charge-time");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("charge-time");
}
