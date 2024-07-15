using Blasphemous.Framework.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.Prayers;

internal class Prayer : ModPrayer
{
    private readonly EffectOnPrayerUse _effect;

    public Prayer(EffectOnPrayerUse effect)
    {
        Id = effect.GetType().Name;
        AddEffect(_effect = effect);
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

    protected override int FervourCost => _effect.FervourCost;
}
