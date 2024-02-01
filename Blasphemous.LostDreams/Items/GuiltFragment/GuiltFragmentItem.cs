using Blasphemous.ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.GuiltFragment;

class GuiltFragmentItem : ModQuestItem
{
    protected override string Id => "QI502";

    protected override string Name => Main.LostDreams.LocalizationHandler.Localize("gfname");

    protected override string Description => Main.LostDreams.LocalizationHandler.Localize("gfdesc");

    protected override string Lore => Main.LostDreams.LocalizationHandler.Localize("gflore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override void LoadImages(out Sprite picture)
    {
        Main.LostDreams.FileHandler.LoadDataAsSprite("guilt-fragment.png", out picture);
    }
}

class GuiltFragmentEffect : ModItemEffectOnAcquire
{
    protected override bool ActivateOnce => false;

    protected override void ApplyEffect() => Main.LostDreams.EffectHandler.Activate("guilt-fragment");

    protected override void RemoveEffect() => Main.LostDreams.EffectHandler.Deactivate("guilt-fragment");
}
