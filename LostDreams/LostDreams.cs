using Framework.Inventory;
using Framework.Managers;
using Gameplay.UI;
using LostDreams.GuiltFragmentBonus;
using ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace LostDreams
{
    public class LostDreams : Mod
    {
        private readonly List<string> _activeEffects = new();

        private string _queuedItem = string.Empty;

        public LostDreams() : base(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION) { }

        protected override void Initialize()
        {
            Log($"{PluginInfo.PLUGIN_NAME} has been initialized");

            RegisterItem(new GuiltFragmentItem().AddEffect<GuiltFragmentEffect>()); // QI502
        }

        protected override void LevelLoaded(string oldLevel, string newLevel)
        {
            if (newLevel == "MainMenu")
            {
                Log("Clearing all item effects");
                _activeEffects.Clear();
                _queuedItem = string.Empty;
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

        public void QueueItem(string item)
        {
            _queuedItem = item;
        }

        public string PeekQueuedItem()
        {
            return _queuedItem;
        }

        public string PopQueuedItem()
        {
            string item = _queuedItem;
            _queuedItem = string.Empty;
            return item;
        }

        public void DisplayItem(string item)
        {
            InventoryManager.ItemType type = ItemModder.GetItemTypeFromId(item);
            BaseInventoryObject obj = Core.InventoryManager.GetBaseObject(item, type);
            if (obj == null) return;

            //obj = Core.InventoryManager.AddBaseObjectOrTears(obj);
            UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GetObejct, obj.caption, obj.picture, type, 3f, true);
        }
    }
}
