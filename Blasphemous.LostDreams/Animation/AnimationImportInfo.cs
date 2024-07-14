
namespace Blasphemous.LostDreams.Animation;

public class AnimationImportInfo
{
    public string Name { get; }
    public string FilePath { get; }
    public Vector Size { get; }
    public Vector Pivot { get; }
    public float SecondsPerFrame { get; }

    public AnimationImportInfo(string name, string filePath, Vector size, Vector pivot, float secondsPerFrame)
    {
        Name = Main.Validate(name, x => !string.IsNullOrEmpty(x));
        FilePath = Main.Validate(filePath, x => !string.IsNullOrEmpty(x));
        Size = Main.Validate(size, x => x.X > 0 && x.Y > 0);
        Pivot = Main.Validate(pivot, x => x.X >= 0 && x.X <= 1 && x.Y >= 0 && x.Y <= 1);
        SecondsPerFrame = Main.Validate(secondsPerFrame, x => x > 0);
    }
}
