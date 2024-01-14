using BepInEx;

namespace LostDreams;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.damocles.blasphemous.modding-api", "1.4.0")]
public class Main : BaseUnityPlugin
{
    public static LostDreams LostDreams { get; private set; }

    private void Start()
    {
        LostDreams = new LostDreams();
    }
}
