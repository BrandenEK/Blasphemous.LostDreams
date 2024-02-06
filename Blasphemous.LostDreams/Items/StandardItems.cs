using Blasphemous.ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items;

public class StandardRosaryBead : ModRosaryBead
{
    public StandardRosaryBead(string id)
    {
        Id = id;
        AddEffect(new StandardEquipEffect(id));
    }

    protected override string Id { get; }

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

public class StandardSwordHeart : ModSwordHeart
{
    public StandardSwordHeart(string id)
    {
        Id = id;
        AddEffect(new StandardEquipEffect(id));
    }

    protected override string Id { get; }

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

public class StandardQuestItem : ModQuestItem
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

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite(Id + ".png", out picture);
    }
}

public class StandardEquipEffect(string effect) : ModItemEffectOnEquip
{
    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate(effect);

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate(effect);
}

public class StandardAcquireEffect(string effect, bool activateOnce) : ModItemEffectOnAcquire
{
    protected override bool ActivateOnce => activateOnce;

    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate(effect);

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate(effect);
}