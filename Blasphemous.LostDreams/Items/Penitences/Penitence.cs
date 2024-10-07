using Blasphemous.Framework.Penitence;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.Penitences;

internal class Penitence : ModPenitenceWithBead
{
    public bool IsActive { get; private set; }

    public Penitence()
    {
        Id = GetType().Name;
        BeadId = $"RB{int.Parse(Id.Substring(2)) + 50}";

        Main.LostDreams.EventHandler.OnExitGame += Deactivate;
    }

    protected override string Id { get; }

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string BeadId { get; }

    protected override PenitenceImageCollection Images
    {
        get
        {
            bool loaded = Main.LostDreams.FileHandler.LoadDataAsVariableSpritesheet($"penitences/{Id}.png",
            [
                new Rect(0, 0, 94, 110),
                new Rect(95, 1, 92, 108),
                new Rect(190, 94, 16, 16),
                new Rect(190, 78, 16, 16),
                new Rect(190, 62, 16, 16),
                new Rect(188, 0, 18, 18)
            ], out Sprite[] images);

            if (!loaded)
                return new PenitenceImageCollection();

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

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }
    protected virtual void OnUpdate() { }

    protected sealed override void Activate()
    {
        IsActive = true;
        OnActivate();
    }

    protected sealed override void Deactivate()
    {
        IsActive = false;
        OnDeactivate();
    }

    protected sealed override void Update()
    {
        if (IsActive)
            OnUpdate();
    }
}