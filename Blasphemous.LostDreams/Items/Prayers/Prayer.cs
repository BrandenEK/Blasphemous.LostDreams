
using Blasphemous.Framework.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.Prayers;

public class Prayer
{
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
