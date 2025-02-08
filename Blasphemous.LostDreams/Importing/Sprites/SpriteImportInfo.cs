using Blasphemous.Framework.Levels;

namespace Blasphemous.LostDreams.Importing.Sprites;

/// <summary>
/// Information on how sprites should be imported
/// </summary>
public class SpriteImportInfo
{
    /// <summary>
    /// The unique identifier of this sprite
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// How many pixels take up one unit of space
    /// </summary>
    public int PixelsPerUnit { get; set; } = 32;

    /// <summary>
    /// Normalized position the sprite is anchored to
    /// </summary>
    public Vector Pivot { get; set; } = new Vector(0.5f, 0.5f, 0.5f);
}
