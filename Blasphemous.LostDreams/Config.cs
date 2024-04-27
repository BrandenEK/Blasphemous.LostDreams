using Blasphemous.LostDreams.Effects;

namespace Blasphemous.LostDreams;

/// <summary>
/// Stores configuration settings
/// </summary>
public class Config
{
    /// <summary> Properties for RB502 </summary>
    public RB502Config RB502 { get; set; } = new();

    /// <summary> Properties for HE501 </summary>
    public HE501Config HE501 { get; set; } = new();

    /// <summary> Properties for LD01 </summary>
    public LD01Config LD01 { get; set; } = new();
}
