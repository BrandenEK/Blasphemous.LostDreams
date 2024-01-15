using Framework.FrameworkCore.Attributes;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using ModdingAPI.Items;
using UnityEngine;

namespace LostDreams.HealthRegen;

class HealthRegenHeart : ModSwordHeart
{
    protected override string Id => "HE501";

    protected override string Name => Main.LostDreams.Localize("hrname");

    protected override string Description => Main.LostDreams.Localize("hrdesc");

    protected override string Lore => Main.LostDreams.Localize("hrlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.LostDreams.FileUtil.loadDataImages("health-regen.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class HealthRegenEffect : ModItemEffectOnEquip
{
    private RawBonus _halfHealth;

    protected override void ApplyEffect()
    {
        Main.LostDreams.TimeHandler.AddTimer("health-regen-half", 0.05f, false, ApplyHalfHealth);
        Main.LostDreams.TimeHandler.AddTimer("health-regen", 3, true, RegenerateHealth);
    }

    protected override void RemoveEffect()
    {
        if (_halfHealth != null)
            Core.Logic.Penitent.Stats.Life.RemoveRawBonus(_halfHealth);

        Main.LostDreams.TimeHandler.RemoveTimer("health-regen");
    }

    private void ApplyHalfHealth()
    {
        _halfHealth = new RawBonus(-Core.Logic.Penitent.Stats.Life.CurrentMax / 2);
        Core.Logic.Penitent.Stats.Life.AddRawBonus(_halfHealth);
    }

    private void RegenerateHealth()
    {
        Life life = Core.Logic.Penitent.Stats.Life;

        if (life.Current >= life.CurrentMax)
            return;

        Main.LostDreams.Log("HE501: Regenerating small health");
        float amount = life.CurrentMax * 0.015f;
        life.Current += amount;
    }
}
