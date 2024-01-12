using LostDreams.GuiltFragmentBonus;
using ModdingAPI;

namespace LostDreams
{
    public class LostDreams : Mod
    {
        public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

        internal AcquisitionHandler AcquisitionHandler { get; } = new();
        internal EffectHandler EffectHandler { get; } = new();

        protected override void Initialize()
        {
            // Register all new items
            RegisterItem(new GuiltFragmentItem().AddEffect<GuiltFragmentEffect>()); // QI502
        }

        protected override void LevelLoaded(string oldLevel, string newLevel)
        {
            if (newLevel != "MainMenu")
                return;

            // Reset handlers when exiting a game
            EffectHandler.Reset();
            AcquisitionHandler.Reset();
        }
    }
}
