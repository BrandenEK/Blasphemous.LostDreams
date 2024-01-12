using LostDreams.GuiltFragmentBonus;
using ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace LostDreams
{
    public class LostDreams : Mod
    {
        private readonly List<string> _activeEffects = new();

        public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

        protected override void Initialize()
        {
            Log($"{PluginInfo.PLUGIN_NAME} has been initialized");

            RegisterItem(new GuiltFragmentItem().AddEffect<GuiltFragmentEffect>()); // QI501
        }

        protected override void LevelLoaded(string oldLevel, string newLevel)
        {
            if (newLevel == "MainMenu")
            {
                Log("Clearing all item effects");
                _activeEffects.Clear();
            }
        }

        protected override void Update()
        {
            if (Time.frameCount % 60 == 0)
                LogWarning("Active: " + IsActive("guilt-fragment"));
        }

        public void Activate(string effect)
        {
            if (!_activeEffects.Contains(effect))
                _activeEffects.Add(effect);
        }

        public void Deactivate(string effect)
        {
            _activeEffects.Remove(effect);
        }

        public bool IsActive(string effect)
        {
            return _activeEffects.Contains(effect);
        }
    }
}
