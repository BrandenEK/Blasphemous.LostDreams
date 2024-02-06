using Blasphemous.ModdingAPI.Levels;
using Blasphemous.ModdingAPI.Levels.Modifiers;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels;

public class ColliderModifier : IModifier
{
    private readonly string _name;
    private readonly Vector2 _size;

    public ColliderModifier(string name, Vector2 size)
    {
        _name = name;
        _size = size;
    }

    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = _name;
        obj.layer = LayerMask.NameToLayer("Floor");

        var collider = obj.AddComponent<BoxCollider2D>();
        collider.size = _size;
    }
}
