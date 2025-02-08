using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// Adds a SpriteRenderer component and sets the properties
/// </summary>
internal class SpriteRendererModifier : IModifier
{
    private static Sprite _square;

    public void Apply(GameObject obj, ObjectData data)
    {
        string spriteName = data.properties[0];
        obj.name = $"Sprite[{spriteName}]";

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.material = new Material(Shader.Find("Sprites/Default"));

        sr.sortingLayerName = data.properties[1];
        sr.sortingOrder = int.Parse(data.properties[2]);
        sr.color = ColorUtility.TryParseHtmlString(data.properties[3], out Color color) ? color : Color.white;
        sr.sprite = spriteName == "__square"
            ? CacheSquareSprite()
            : Main.LostDreams.SpriteStorage[spriteName];
    }

    private static Sprite CacheSquareSprite()
    {
        if (_square == null)
        {
            Texture2D tex = Texture2D.whiteTexture;
            _square = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, tex.width);
        }

        return _square;
    }
}
