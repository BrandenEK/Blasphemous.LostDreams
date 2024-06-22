using UnityEngine;

namespace Blasphemous.LostDreams.Animation;

public class AnimationInfo
{
    public string Name { get; }
    public Sprite[] Sprites { get; }
    public float SecondsPerFrame { get; }

    public AnimationInfo(string name, Sprite[] sprites, float secondsPerFrame)
    {
        Name = Main.Validate(name, x => !string.IsNullOrEmpty(x));
        Sprites = Main.Validate(sprites, x => x != null && x.Length > 0);
        SecondsPerFrame = Main.Validate(secondsPerFrame, x => x > 0);
    }
}
