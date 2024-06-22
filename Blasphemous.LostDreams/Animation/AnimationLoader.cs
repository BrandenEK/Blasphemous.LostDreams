using Blasphemous.ModdingAPI.Files;

namespace Blasphemous.LostDreams.Animation;

public class AnimationLoader
{
    public AnimationLoader(FileHandler file)
    {
        string infoPath = "animations.json";
        if (!file.LoadDataAsJson(infoPath, out AnimationImportInfo[] imports))
        {
            Main.LostDreams.LogError("Failed to load animation list");
            return;
        }

        foreach (var importInfo in imports)
        {
            Main.LostDreams.LogWarning(importInfo.Name + " from " + importInfo.FilePath);
        }
    }
}
