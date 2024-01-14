using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.DamageStack;

class DamageStackBead : ModRosaryBead
{
    protected override string Id => "RB502";

    protected override string Name => Main.LostDreams.Localize("dsname");

    protected override string Description => Main.LostDreams.Localize("dsdesc");

    protected override string Lore => Main.LostDreams.Localize("dslore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("damage-stack.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class DamageStackEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("damage-stack");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("damage-stack");
}
