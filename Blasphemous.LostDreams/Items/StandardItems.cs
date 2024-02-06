using Blasphemous.ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items;

public class StandardRosaryBead(string id) : ModRosaryBead
{
    protected override string Id { get; } = id;

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite(Id + ".png", out picture);
    }
}

public class StandardSwordHeart(string id) : ModSwordHeart
{
    protected override string Id { get; } = id;

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override bool AddToPercentCompletion => true;

    protected override bool AddInventorySlot => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite(Id + ".png", out picture);
    }
}

public class StandardQuestItem(string id) : ModQuestItem
{
    protected override string Id { get; } = id;

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite(Id + ".png", out picture);
    }
}
