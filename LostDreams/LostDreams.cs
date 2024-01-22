using LostDreams.GuiltFragmentBonus;
﻿using LostDreams.ChargeTimeDecrease;
﻿using LostDreams.DamageRemovalOnce;
using ModdingAPI;
using LostDreams.FlaskHealth;

namespace LostDreams;

public class LostDreams : Mod
{
    public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    internal AcquisitionHandler AcquisitionHandler { get; } = new();
    internal EffectHandler EffectHandler { get; } = new();

    protected override void Initialize()
    {
        // Register all new items
        RegisterItem(new ChargeTimeBead().AddEffect<ChargeTimeEffect>()); // RB501
        RegisterItem(new DamageRemovalBead().AddEffect<DamageRemovalEffect>()); // RB503
        RegisterItem(new FlaskHealthBead().AddEffect<FlaskHealthEffect>()); // RB504

        RegisterItem(new GuiltFragmentItem().AddEffect<GuiltFragmentEffect>()); // QI502
    }

    protected override void LevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel != "MainMenu")
            return;

        // Reset handlers when exiting a game
        EffectHandler.Reset();
        AcquisitionHandler.Reset();

        // Handle extra functionality for certain items
        Log("RB503: Regain damage removal (mainmenu)");
        DamageRemovalEffect.RegainDamageRemoval();
    }
}
