using Framework.FrameworkCore.Attributes;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.SwordHearts;

internal class HE501 : EquipEffect
{
    private readonly HE501Config _config;

    private RawBonus _halfHealth;
    private float _nextHealTime;
    private bool _applyNextFrame;

    public HE501(HE501Config config)
    {
        _config = config;
    }

    protected override void ApplyEffect()
    {
        _applyNextFrame = true;
        _nextHealTime = Time.time + _config.REGEN_DELAY;
    }

    protected override void RemoveEffect()
    {
        if (_halfHealth != null)
            Core.Logic.Penitent.Stats.Life.RemoveRawBonus(_halfHealth);
    }

    protected override void Update()
    {
        if (_applyNextFrame)
        {
            _applyNextFrame = false;
            ApplyHalfHealth();
        }

        if (Time.time >= _nextHealTime)
        {
            RegenerateHealth();
            _nextHealTime = Time.time + _config.REGEN_DELAY;
        }
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