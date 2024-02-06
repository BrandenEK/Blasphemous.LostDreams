using Blasphemous.ModdingAPI.Penitence;
using Framework.Managers;
using UnityEngine;

namespace Blasphemous.LostDreams.Penitences;

public class StandardPenitence(string id, string item) : ModPenitence
{
    protected override string Id { get; } = id;

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string ItemIdToGive { get; } = item;

    protected override InventoryManager.ItemType ItemTypeToGive => InventoryManager.ItemType.Bead;

    protected override void Activate() => Main.LostDreams.PenitenceHandler.Activate(Id);

    protected override void Deactivate() => Main.LostDreams.PenitenceHandler.Deactivate(Id);

    protected override void LoadImages(out Sprite inProgress, out Sprite completed, out Sprite abandoned, out Sprite gameplay, out Sprite chooseSelected, out Sprite chooseUnselected)
    {
        Main.LostDreams.FileHandler.LoadDataAsVariableSpritesheet(CurrentId + ".png",
        [
            new Rect(0, 0, 94, 110),
            new Rect(95, 1, 92, 108),
            new Rect(190, 0, 16, 16),
            new Rect(190, 16, 16, 16),
            new Rect(190, 32, 16, 16),
            new Rect(188, 92, 18, 18)
        ], out Sprite[] images);

        chooseSelected = images[0];
        chooseUnselected = images[1];
        inProgress = images[2];
        completed = images[3];
        abandoned = images[4];
        gameplay = images[5];
    }

    public static string CurrentId { get; set; }
}
