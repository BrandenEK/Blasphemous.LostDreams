using Blasphemous.Framework.Levels;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// A modifier that adds a box collider to an object
/// </summary>
internal class BoxColliderModifier : BaseColliderModifier
{
    private readonly Vector2 _size;

    /// <summary>
    /// Specify the layer, size, and offset of the collider
    /// </summary>
    public BoxColliderModifier(string layer, Vector2 size, Vector2 offset) : base(layer, offset)
    {
        _size = size;
    }

    /// <summary>
    /// Specify the size of the collider, offset set to (0, 0)
    /// </summary>
    public BoxColliderModifier(string layer, Vector2 size) : this(layer, size, new Vector2(0, 0)) { }

    public override void Apply(GameObject obj, ObjectData data)
    {
        base.Apply(obj, data);

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
        collider.offset = _offset;

        // if there's a color property, color the collider
        if (TryGetColorFromObjectData(data, out Color color))
        {
            GenerateColoredMesh(color, ref obj, out Mesh mesh, out MeshFilter mf, out MeshRenderer mr);

            // move the mesh to the same position and size of the collider
            Vector3[] vertices =
            [
            new Vector3(-0.5f * _size.x, -0.5f * _size.y) + (Vector3)_offset, // bottom left
            new Vector3(0.5f * _size.x, -0.5f * _size.y) + (Vector3)_offset,  // bottom right
            new Vector3(0.5f * _size.x, 0.5f * _size.y) + (Vector3)_offset,   // top right
            new Vector3(-0.5f * _size.x, 0.5f * _size.y) + (Vector3)_offset   // top left
            ];

            int[] triangles =
                [
                0, 2, 1,
                0, 3, 2
                ];

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mr.allowOcclusionWhenDynamic = true;
            mr.receiveShadows = true;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            mr.sortingOrder = -20000000;
        }
    }
}
