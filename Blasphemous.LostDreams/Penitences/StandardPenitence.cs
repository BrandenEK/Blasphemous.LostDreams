using Blasphemous.Framework.Penitence;
using UnityEngine;

namespace Blasphemous.LostDreams.Penitences;

internal class StandardPenitence(string id, string item) : ModPenitenceWithBead
{
    protected override string Id { get; } = id;

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string BeadId { get; } = item;

    protected override PenitenceImageCollection Images
    {
        get
        {
            Main.LostDreams.FileHandler.LoadDataAsVariableSpritesheet($"penitences/{Id}.png",
            [
                new Rect(0, 0, 94, 110),
                new Rect(95, 1, 92, 108),
                new Rect(190, 94, 16, 16),
                new Rect(190, 78, 16, 16),
                new Rect(190, 62, 16, 16),
                new Rect(188, 0, 18, 18)
            ], out Sprite[] images);

            return new PenitenceImageCollection()
            {
                ChooseSelected = images[0],
                ChooseUnselected = images[1],
                InProgress = images[2],
                Completed = images[3],
                Abandoned = images[4],
                Gameplay = images[5]
            };
        }
    }
    protected override void Activate() => Main.LostDreams.PenitenceHandler.Activate(Id);

    protected override void Deactivate() => Main.LostDreams.PenitenceHandler.Deactivate(Id);
}
