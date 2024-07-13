using Blasphemous.LostDreams.Items.Penitences;
using Blasphemous.LostDreams.Items.RosaryBeads;
using Blasphemous.LostDreams.Items.SwordHearts;

namespace Blasphemous.LostDreams;

/// <summary>
/// Stores configuration settings
/// </summary>
public class Config
{
    /// <summary> Properties for RB502 </summary>
    public RB502Config RB502 { get; set; } = new();
    /// <summary> Properties for RB510 </summary>
    public RB510Config RB510 { get; set; } = new();
    /// <summary> Properties for RB511 </summary>
    public RB511Config RB511 { get; set; } = new();
    /// <summary> Properties for RB512 </summary>
    public RB512Config RB512 { get; set; } = new();
    /// <summary> Properties for RB513 </summary>
    public RB513Config RB513 { get; set; } = new();
    /// <summary> Properties for RB514 </summary>
    public RB514Config RB514 { get; set; } = new();

    /// <summary> Properties for HE501 </summary>
    public HE501Config HE501 { get; set; } = new();

    /// <summary> Properties for PE501 </summary>
    public PE501Config PE501 { get; set; } = new();
}
