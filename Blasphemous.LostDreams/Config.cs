
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

    /// <summary> When hitting an enemy with the sword, restore health by damage times this multiplier </summary>
    public float LD01_SWORD_HEAL_PERCENT = 0.2f;
    /// <summary> When hitting an enemy with anything else, restore health by this amount </summary>
    public float LD01_PRAYER_HEAL_AMOUNT = 3f;
    /// <summary> Amount of health restored when killing an enemy </summary>
    public float LD01_KILL_HEAL_AMOUNT = 15f;
    /// <summary> Number of seconds between each health drain tick </summary>
    public float LD01_DRAIN_DELAY = 2f;
    /// <summary> Base amount of health lost every tick </summary>
    public float LD01_DRAIN_BASE = 2f;
    /// <summary> Additional amount per health upgrade of health lost every tick </summary>
    public float LD01_DRAIN_INCREASE = 2f;
    /// <summary> Damage applied to enemies when the player is hit </summary>
    public float LD01_THORNS_AMOUNT = 10f;
    /// <summary> Damage applied to the player in place of contact damage </summary>
    public float LD01_CONTACT_AMOUNT = 3f;
    /// <summary> Base time that drinking a flask stops health drain </summary>
    public float LD01_FLASK_BASE = 5f;
    /// <summary> Additional time per level that drinking a flask stops health drain </summary>
    public float LD01_FLASK_INCREASE = 1f;
}
