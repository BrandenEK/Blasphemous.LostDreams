using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.FlaskHealth;

class FlaskHealthBead : ModRosaryBead
{
    protected override string Id => "RB504";

    protected override string Name => Main.LostDreams.Localize("fhname");

    protected override string Description => Main.LostDreams.Localize("fhdesc");

    protected override string Lore => Main.LostDreams.Localize("fhlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("flask-health.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class FlaskHealthEffect : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("flask-health");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("flask-health");
}
