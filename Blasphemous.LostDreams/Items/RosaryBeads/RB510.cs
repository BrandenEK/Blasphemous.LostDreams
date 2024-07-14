using Gameplay.GameControllers.Entities;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB510(RB510Config config) : EffectOnEquipStat(EntityStats.StatsTypes.FlaskHealth, config.FLASK_HEAL_BUFF)
{
}

/// <summary>
/// Properties for RB510
/// </summary>
public class RB510Config
{
    /// <summary> How much extra should you be healed when using a flask </summary>
    public float FLASK_HEAL_BUFF = 20f;
}