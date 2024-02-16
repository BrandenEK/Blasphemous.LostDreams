using BepInEx;

namespace Blasphemous.LostDreams;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.0.2")]
public class Main : BaseUnityPlugin
{
    public static LostDreams LostDreams { get; private set; }

    private void Start()
    {
        LostDreams = new LostDreams();
    }
}
