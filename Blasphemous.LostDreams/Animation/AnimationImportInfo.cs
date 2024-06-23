
namespace Blasphemous.LostDreams.Animation;

public class AnimationImportInfo
{
    public string Name { get; }
    public string FilePath { get; }
    public int Width { get; }
    public int Height { get; }
    public float SecondsPerFrame { get; }

    public AnimationImportInfo(string name, string filePath, int width, int height, float secondsPerFrame)
    {
        Name = Main.Validate(name, x => !string.IsNullOrEmpty(x));
        FilePath = Main.Validate(filePath, x => !string.IsNullOrEmpty(x));
        Width = Main.Validate(width, x => x > 0);
        Height = Main.Validate(height, x => x > 0);
        SecondsPerFrame = Main.Validate(secondsPerFrame, x => x > 0);
    }
}
