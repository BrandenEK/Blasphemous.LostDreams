using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB513 : EffectOnEquip
{
    private bool _isActive = false;
    private float _currentTimer = 0f;

    private readonly RawBonus _damageReductionBonus;
    private readonly float _effectDuration;

    public RB513(RB513Config config)
    {
        _damageReductionBonus = new(config.DAMAGE_REDUCTION_BUFF);
        _effectDuration = config.DURATION;

        Main.LostDreams.EventHandler.OnUseFlask += ApplyBonus;
    }

    private void ApplyBonus(ref bool cancel)
    {
        if (!IsEquipped)
            return;

        Core.Logic.Penitent.Stats.NormalDmgReduction.AddRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.MagicDmgReduction.AddRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.LightningDmgReduction.AddRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.FireDmgReduction.AddRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.ToxicDmgReduction.AddRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.ContactDmgReduction.AddRawBonus(_damageReductionBonus);

        _isActive = true;
        _currentTimer = 0f;

        Main.LostDreams.Log($"Applied RB513 defense bonus!");
    }

    private void RemoveBonus()
    {
        Core.Logic.Penitent.Stats.NormalDmgReduction.RemoveRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.MagicDmgReduction.RemoveRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.LightningDmgReduction.RemoveRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.FireDmgReduction.RemoveRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.ToxicDmgReduction.RemoveRawBonus(_damageReductionBonus);
        Core.Logic.Penitent.Stats.ContactDmgReduction.RemoveRawBonus(_damageReductionBonus);

        _isActive = false;
        _currentTimer = 0f;

        Main.LostDreams.Log($"Removed RB513 defense bonus!");
    }

    protected override void OnUpdate()
    {
        if (!_isActive)
            return;

        _currentTimer += Time.deltaTime;
        if (_currentTimer >= _effectDuration)
        {
            RemoveBonus();
        }
    }

}

/// <summary> 
/// Properties for RB513 
/// </summary>
public class RB513Config
{
    /// <summary> Percent that every damage type should be reduced by </summary>
    public float DAMAGE_REDUCTION_BUFF = 0.4f;
    /// <summary> How many seconds should the effect last after using a flask </summary>
    public float DURATION = 10f;
}