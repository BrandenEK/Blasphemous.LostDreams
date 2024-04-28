using Blasphemous.Framework.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items;

internal class StandardRosaryBead : ModRosaryBead
{
    public StandardRosaryBead(string id, bool useEffect)
    {
        Id = id;
        if (useEffect)
            AddEffect(new StandardEquipEffect(id));
    }

    protected override string Id { get; }

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override Sprite Picture => Main.LostDreams.FileHandler.LoadDataAsSprite($"beads/{Id}.png", out Sprite picture) ? picture : null;

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;
}

internal class StandardPrayer : ModPrayer
{
    public StandardPrayer(string id, int fervour)
    {
        Id = id;
        FervourCost = fervour;
    }

    protected override string Id { get; }

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override Sprite Picture => Main.LostDreams.FileHandler.LoadDataAsSprite($"prayers/{Id}.png", out Sprite picture) ? picture : null;

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override int FervourCost { get; }
}

internal class StandardSwordHeart : ModSwordHeart
{
    public StandardSwordHeart(string id, bool useEffect)
    {
        Id = id;
        if (useEffect)
            AddEffect(new StandardEquipEffect(id));
    }

    protected override string Id { get; }

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override Sprite Picture => Main.LostDreams.FileHandler.LoadDataAsSprite($"swords/{Id}.png", out Sprite picture) ? picture : null;

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;
}

internal class StandardQuestItem : ModQuestItem
{
    public StandardQuestItem(string id, bool activateOnce)
    {
        Id = id;
        AddEffect(new StandardAcquireEffect(id, activateOnce));
    }

    protected override string Id { get; }

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override Sprite Picture => Main.LostDreams.FileHandler.LoadDataAsSprite($"questitems/{Id}.png", out Sprite picture) ? picture : null;

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;
}

internal class StandardEquipEffect(string effect) : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.ItemHandler.Equip(effect);

    protected override void RemoveEffect() => Main.LostDreams.ItemHandler.Unequip(effect);
}

internal class StandardAcquireEffect(string effect, bool activateOnce) : ModItemEffectOnAcquire
{
    protected override bool ActivateOnce => activateOnce;

    protected override void ApplyEffect() => Main.LostDreams.ItemHandler.Equip(effect);

    protected override void RemoveEffect() => Main.LostDreams.ItemHandler.Unequip(effect);
}
