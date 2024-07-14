using UnityEngine;

namespace Blasphemous.LostDreams;

/// <summary>
/// A vector containing an x and a y component
/// </summary>
public class Vector
{
    /// <summary>
    /// The x component
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// The y component
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Casts a Vector to a UnityEngine Vector2
    /// </summary>
    public static implicit operator Vector2(Vector v) => new(v.X, v.Y);

    /// <summary>
    /// Casts a Vector to a UnityEngine Vector3
    /// </summary>
    public static implicit operator Vector3(Vector v) => new(v.X, v.Y, 0);
}
