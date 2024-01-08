using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.GuiltFragmentBonus;

class GuiltFragmentBead : ModRosaryBead
{
    protected override string Id => "RB502";

    protected override string Name => Main.LostDreams.Localize("gfname");

    protected override string Description => Main.LostDreams.Localize("gfdesc");

    protected override string Lore => Main.LostDreams.Localize("gflore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("guilt-fragment.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class GuiltFragmentEffect : ModItemEffectOnEquip
{
    public static bool Active { get; private set; }

    protected override void ApplyEffect() => Active = true;

    protected override void RemoveEffect() => Active = false;
}
