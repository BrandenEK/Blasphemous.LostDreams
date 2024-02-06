using System.Collections.Generic;

namespace Blasphemous.LostDreams.Items;

internal class ItemHandler
{
    private readonly List<string> _equipped = new();

    public void Equip(string item)
    {
        if (!_equipped.Contains(item))
        {
            Main.LostDreams.Log("Equipping item: " + item);
            _equipped.Add(item);
        }
    }

    public void Unequip(string item)
    {
        Main.LostDreams.Log("Unequipping item: " + item);
        _equipped.Remove(item);
    }

    public void Reset()
    {
        Main.LostDreams.Log("Clearing all equipped items");
        _equipped.Clear();
    }

    public bool IsEquipped(string item)
    {
        return _equipped.Contains(item);
    }
}
