using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using UnityEngine;

namespace Blasphemous.LostDreams.Levels.Modifiers;

/// <summary>
/// Creates a SpriteRenderer with a solid color
/// </summary>
internal class ColorRectangleModifier : IModifier
{
    private static Sprite _square;

    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = "ColoredRectangle";

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.material = new Material(Shader.Find("Sprites/Default"));
        sr.sprite = CacheSquareSprite();

        sr.sortingLayerName = data.properties[0];
        sr.sortingOrder = int.Parse(data.properties[1]);
        sr.color = ColorUtility.TryParseHtmlString(data.properties[2], out Color color) ? color : Color.white;
    }

    private static Sprite CacheSquareSprite()
    {
        if (_square == null)
        {
            Texture2D tex = Texture2D.whiteTexture;
            _square = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 1);
        }

        return _square;
    }
}
