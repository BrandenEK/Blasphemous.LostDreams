using Framework.Inventory;
using Framework.Managers;
using Gameplay.UI;
using ModdingAPI;

namespace LostDreams;

internal class AcquisitionHandler
{
    private BaseInventoryObject _queuedItem = null;

    public void GiveItem(string item, bool skipIfOwned, bool queueDisplay)
    {
        BaseInventoryObject obj = GetObjectFromId(item);
        if (obj == null)
            return;

        Main.LostDreams.Log("Acquiring item: " + item);
        if (skipIfOwned && Core.InventoryManager.IsBaseObjectEquipped(obj))
            return;

        _queuedItem = Core.InventoryManager.AddBaseObjectOrTears(obj);

        if (!queueDisplay)
            DisplayQueuedItem();
    }

    public void DisplayQueuedItem()
    {
        if (_queuedItem == null)
            return;

        BaseInventoryObject obj = _queuedItem;
        _queuedItem = null;

        UIController.instance.ShowObjectPopUp(UIController.PopupItemAction.GetObejct, obj.caption, obj.picture, obj.GetItemType(), 3f, true);
    }

    private BaseInventoryObject GetObjectFromId(string item)
    {
        InventoryManager.ItemType type = ItemModder.GetItemTypeFromId(item);
        return Core.InventoryManager.GetBaseObject(item, type);
    }

    public void Reset()
    {
        _queuedItem = null;
    }
}
