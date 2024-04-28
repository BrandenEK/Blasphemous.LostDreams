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
    private readonly HE501Config _config;

    private RawBonus _halfHealth;

    /// <summary>
    /// Initializes regen parameters
    /// </summary>
    public HealthRegen(HE501Config config)
    {
        _config = config;
    }

    /// <summary>
    /// Add timer for regen, but almost immediately half health
    /// </summary>
    protected override void ApplyEffect()
    {
        Main.LostDreams.TimeHandler.AddCountdown("health-regen-half", 0.05f, ApplyHalfHealth);
        Main.LostDreams.TimeHandler.AddTicker("health-regen", _config.REGEN_DELAY, false, RegenerateHealth);
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
        float amount = life.CurrentMax * _config.REGEN_PERCENT;
        life.Current += amount;
    }
}

/// <summary> Properties for HE501 </summary>
public class HE501Config
{
    /// <summary> The decimal multiplied by your maximum health to heal every tick </summary>
    public float REGEN_PERCENT = 0.015f;
    /// <summary> How many seconds in between each heal tick </summary>
    public float REGEN_DELAY = 1f;
}
