
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

    /// <summary> Amount of health restored when hitting an enemy </summary>
    public float LD01_HIT_HEAL_AMOUNT = 5f;
    /// <summary> Amount of health restored when killing an enemy </summary>
    public float LD01_KILL_HEAL_AMOUNT = 15f;
    /// <summary> Number of seconds between each health drain tick </summary>
    public float LD01_DRAIN_DELAY = 2f;
    /// <summary> Amount of health lost every tick </summary>
    public float LD01_DRAIN_AMOUNT = 2f;
    /// <summary> Damage applied to enemies when the player is hit </summary>
    public float LD01_THORNS_AMOUNT = 10f;
    /// <summary> Damage applied to the player in place of contact damage </summary>
    public float LD01_CONTACT_AMOUNT = 3f;
    /// <summary> Length in seconds of the speed increase from drinking a flask </summary>
    public float LD01_SPEED_LENGTH = 30f;
    /// <summary> Speed increase gained from drinking a flask </summary>
    public float LD01_SPEED_AMOUNT = 30f;
}
