using LostDreams.GuiltFragment;
using LostDreams.ChargeTime;
using LostDreams.DamageRemoval;
using ModdingAPI;
using LostDreams.DamageStack;
using LostDreams.Events;

namespace LostDreams;

public class LostDreams : Mod
{
    public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    internal AcquisitionHandler AcquisitionHandler { get; } = new();
    internal EffectHandler EffectHandler { get; } = new();
    internal EventHandler EventHandler { get; } = new();

    protected override void Initialize()
    {
        // Register all new items
        RegisterItem(new ChargeTimeBead().AddEffect<ChargeTimeEffect>()); // RB501
        RegisterItem(new DamageStackBead().AddEffect<DamageStackEffect>()); // RB502
        RegisterItem(new DamageRemovalBead().AddEffect<DamageRemovalEffect>()); // RB503

        RegisterItem(new GuiltFragmentItem().AddEffect<GuiltFragmentEffect>()); // QI502
    }

    protected override void LevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel != "MainMenu")
            return;

        // Reset handlers when exiting a game
        EffectHandler.Reset();
        AcquisitionHandler.Reset();
        EventHandler.Reset();
    }
}
