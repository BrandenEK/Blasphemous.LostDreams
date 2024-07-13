using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB510 : EffectOnEquip 
{
    private readonly RawBonus _flaskHealBonus;

    public RB510(RB510Config config)
    {
        _flaskHealBonus = new(config.FLASK_HEAL_BUFF);
    }

    protected override void OnEquip()
    {
        Core.Logic.Penitent.Stats.FlaskHealth.AddRawBonus(_flaskHealBonus);
    }

    protected override void OnUnequip()
    {
        Core.Logic.Penitent.Stats.FlaskHealth.RemoveRawBonus(_flaskHealBonus);
    }
}

/// <summary> 
/// Properties for RB510 
/// </summary>
public class RB510Config
{
    /// <summary> How much extra should you be healed when using a flask </summary>
    public float FLASK_HEAL_BUFF = 20f;
}