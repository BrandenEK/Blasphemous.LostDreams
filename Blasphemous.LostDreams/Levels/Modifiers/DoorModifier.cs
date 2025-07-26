using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Framework.FrameworkCore;
using Tools.Level.Interactables;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// Sets the required properties for a door object
/// </summary>
internal class DoorModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = $"Door[{data.id}]";

        Door door = obj.GetComponent<Door>();
        door.identificativeName = data.id;
        door.targetScene = data.properties[0];
        door.targetDoor = data.properties[1];
        door.exitOrientation = data.properties[2] switch
        {
            "left" => EntityOrientation.Left,
            "right" => EntityOrientation.Right,
            _ => throw new System.Exception("Invalid door orientation")
        };
        door.spawnPoint.transform.position = new()
        {
            x = float.Parse(data.properties[3]),
            y = float.Parse(data.properties[4]),
        };
    }
}
