using Blasphemous.Framework.Items;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.FrameworkCore.Attributes;
using Framework.Managers;

namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// Cuts health in half, but applies a timer for regen
/// </summary>
public class HealthRegen : ModItemEffectOnEquip
{
    private RawBonus _halfHealth;

    /// <summary>
    /// Add timer for regen, but almost immediately half health
    /// </summary>
    protected override void ApplyEffect()
    {
        Main.LostDreams.TimeHandler.AddTimer("health-regen-half", 0.05f, false, ApplyHalfHealth);
        Main.LostDreams.TimeHandler.AddTimer("health-regen", REGEN_DELAY, true, RegenerateHealth);
    }

    /// <summary>
    /// Remove half health and stop timer for regen
    /// </summary>
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
