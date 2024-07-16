
namespace Blasphemous.LostDreams.Items.Prayers;

/// <summary>
/// Defines properties required for all prayers
/// </summary>
public interface IPrayerConfig
{
    /// <summary>
    /// How much fervour is required to use this prayer
    /// </summary>
    public int FervourCost { get; }

    /// <summary>
    /// How long the prayer should last for
    /// </summary>
    public float EffectTime { get; }

    /// <summary>
    /// Whether the effect time should be modified by player stats
    /// </summary>
    public bool UsePrayerDurationModifier { get; }
}
