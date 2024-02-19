using BepInEx;

namespace Blasphemous.LostDreams;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.1.0")]
[BepInDependency("Blasphemous.Framework.Items", "0.1.0")]
[BepInDependency("Blasphemous.Framework.Levels", "0.1.0")]
[BepInDependency("Blasphemous.Framework.Penitence", "0.1.0")]
internal class Main : BaseUnityPlugin
{
    public static LostDreams LostDreams { get; private set; }

    private void Start()
    {
        LostDreams = new LostDreams();
    }
}
