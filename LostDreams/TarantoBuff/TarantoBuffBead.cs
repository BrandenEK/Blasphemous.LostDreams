using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.TarantoBuff;

class TarantoBuffBead : ModRosaryBead
{
    protected override string Id => "RB504";

    protected override string Name => Main.LostDreams.Localize("tbname");

    protected override string Description => Main.LostDreams.Localize("tbdesc");

    protected override string Lore => Main.LostDreams.Localize("tblore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("taranto-buff.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class TarantoBuffEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("taranto-buff");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("taranto-buff");
}
