using Blasphemous.LostDreams.Beads;
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

    /// <summary> Properties for PE501 </summary>
    public PE501Config PE501 { get; set; } = new();
}
