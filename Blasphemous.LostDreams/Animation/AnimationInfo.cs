using UnityEngine;

namespace Blasphemous.LostDreams.Animation;

public class AnimationInfo
{
    public string Name { get; }
    public Sprite[] Sprites { get; }
    public float SecondsPerFrame { get; }

    public AnimationInfo(string name, Sprite[] sprites, float secondsPerFrame)
    {
        Name = name;
        Sprites = sprites;
        SecondsPerFrame = secondsPerFrame;
    }
}
