using LostDreams.ChargeTimeDecrease;
﻿using LostDreams.DamageRemovalOnce;
using ModdingAPI;

namespace LostDreams;

public class LostDreams : Mod
{
    public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

    protected override void Initialize()
    {
        Log($"{PluginInfo.PLUGIN_NAME} has been initialized");

        RegisterItem(new ChargeTimeBead().AddEffect<ChargeTimeEffect>()); // RB501
        RegisterItem(new DamageRemovalBead().AddEffect<DamageRemovalEffect>()); // RB503
    }

    protected override void LevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel == "MainMenu")
        {
            Log("RB503: Regain damage removal (mainmenu)");
            DamageRemovalEffect.RegainDamageRemoval();
        }
    }
}
