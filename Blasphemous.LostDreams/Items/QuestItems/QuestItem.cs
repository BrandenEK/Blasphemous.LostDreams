using Blasphemous.Framework.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.QuestItems;

internal class QuestItem : ModQuestItem
{
    private readonly EffectOnAcquire _effect;

    public bool IsAcquired => _effect.IsAcquired;

    public QuestItem(EffectOnAcquire effect)
    {
        Id = effect.GetType().Name;
        AddEffect(_effect = effect);
    }

    protected override string Id { get; }

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize(Id + ".n");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize(Id + ".d");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize(Id + ".l");

    protected override Sprite Picture => Main.LostDreams.FileHandler.LoadDataAsSprite($"questitems/{Id}.png", out Sprite picture) ? picture : null;

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;
}
