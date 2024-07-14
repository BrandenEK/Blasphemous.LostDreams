using Gameplay.GameControllers.Entities;

namespace Blasphemous.LostDreams.Items.RosaryBeads;

internal class RB511(RB511Config config) : EffectOnEquipStat(EntityStats.StatsTypes.Flask, config.FLASK_AMOUNT_BUFF)
{
}

/// <summary>
/// Properties for RB511
/// </summary>
public class RB511Config
{
    /// <summary> How many extra flasks you should be given </summary>
    public int FLASK_AMOUNT_BUFF = 1;
}