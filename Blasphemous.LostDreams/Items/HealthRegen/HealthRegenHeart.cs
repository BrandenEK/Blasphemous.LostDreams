using Blasphemous.ModdingAPI.Items;
using Framework.FrameworkCore.Attributes;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.HealthRegen;

class HealthRegenHeart : ModSwordHeart
{
    protected override string Id => "HE501";

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize("hrname");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize("hrdesc");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize("hrlore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite("health-regen.png", out picture);
    }
}

class HealthRegenEffect : ModItemEffectOnEquip
{
    private RawBonus _halfHealth;

    protected override void ApplyEffect()
    {
        Main.LostDreams.TimeHandler.AddTimer("health-regen-half", 0.05f, false, ApplyHalfHealth);
        Main.LostDreams.TimeHandler.AddTimer("health-regen", REGEN_DELAY, true, RegenerateHealth);
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

        //Main.LostDreams.Log("HE501: Regenerating small health");
        float amount = life.CurrentMax * REGEN_PERCENT;
        life.Current += amount;
    }

    private const float REGEN_PERCENT = 0.015f;
    private const float REGEN_DELAY = 1f;
}
