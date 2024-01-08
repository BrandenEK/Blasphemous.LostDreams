using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.DamageRemovalOnce;

class DamageRemovalBead : ModRosaryBead
{
    protected override string Id => "RB503";

    protected override string Name => Main.LostDreams.Localize("drname");

    protected override string Description => Main.LostDreams.Localize("drdesc");

    protected override string Lore => Main.LostDreams.Localize("drlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("damage-removal.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class DamageRemovalEffect : ModItemEffectOnEquip
{
    public static bool Active { get; private set; }

    public static bool WillRemoveDamage { get; private set; }

    public static bool HealingFlag { get; set; }

    protected override void ApplyEffect() => Active = true;

    protected override void RemoveEffect() => Active = false;

    public static void RegainDamageRemoval()
    {
        WillRemoveDamage = true;
    }

    public static void UseDamageRemoval()
    {
        WillRemoveDamage = false;
    }
}
