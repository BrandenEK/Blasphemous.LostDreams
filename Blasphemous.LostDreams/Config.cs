
namespace Blasphemous.LostDreams;

/// <summary>
/// Stores configuration settings
/// </summary>
public class Config
{
    /// <summary> The total number of charges you can go up to </summary>
    public int RB502_MAX_CHARGES = 20;
    /// <summary> The multiplier added to damage when you have maximum charges </summary>
    public float RB502_MAX_MULTIPLIER = 2.0f;

    /// <summary> The decimal multiplied by your maximum health to heal every tick </summary>
    public float HE501_REGEN_PERCENT = 0.015f;
    /// <summary> How many seconds in between each heal tick </summary>
    public float HE501_REGEN_DELAY = 1f;
}
