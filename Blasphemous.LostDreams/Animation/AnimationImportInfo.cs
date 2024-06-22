
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
        Name = name;
        FilePath = filePath;
        Width = width;
        Height = height;
        SecondsPerFrame = secondsPerFrame;
    }
}
