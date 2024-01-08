using ModdingAPI;

namespace LostDreams
{
    public class LostDreams : Mod
    {
        public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

        protected override void Initialize()
        {
            Log($"{PluginInfo.PLUGIN_NAME} has been initialized");
        }
    }
}
