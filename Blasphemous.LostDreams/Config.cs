
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

    /// <summary> Base damage multiplier to heal after sword hit </summary>
    public float LD01_HEAL_SWORD_BASE = 0.1f;
    /// <summary> Additional damage multiplier to heal after sword hit </summary>
    public float LD01_HEAL_SWORD_INCREASE = 0.015f;

    /// <summary> Base amount to heal after prayer hit </summary>
    public float LD01_HEAL_PRAYER_BASE = 3f;
    /// <summary> Additional amount to heal after prayer hit </summary>
    public float LD01_HEAL_PRAYER_INCREASE = 0.45f;

    /// <summary> Base amount to heal after killing enemy </summary>
    public float LD01_HEAL_KILL_BASE = 15f;
    /// <summary> Additional amount to heal after killing enemy </summary>
    public float LD01_HEAL_KILL_INCREASE = 2.5f;

    /// <summary> Base time that drinking a flask stops health drain </summary>
    public float LD01_FLASK_BASE = 10f;
    /// <summary> Additional time per level that drinking a flask stops health drain </summary>
    public float LD01_FLASK_INCREASE = 6f;

    /// <summary> Number of seconds between each health drain tick </summary>
    public float LD01_DRAIN_DELAY = 2f;
    /// <summary> Base amount of health lost every tick </summary>
    public float LD01_DRAIN_BASE = 4f;
    /// <summary> Additional amount per health upgrade of health lost every tick </summary>
    public float LD01_DRAIN_INCREASE = 0.75f;

    /// <summary> Damage applied to enemies when the player is hit </summary>
    public float LD01_THORNS_AMOUNT = 40f;
    /// <summary> Damage applied to the player in place of contact damage </summary>
    public float LD01_CONTACT_AMOUNT = 3f;
}
