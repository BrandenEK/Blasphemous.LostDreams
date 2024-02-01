using ModdingAPI.Items;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.GuiltFragment;

class GuiltFragmentItem : ModQuestItem
{
    protected override string Id => "QI502";

    protected override string Name => Main.Blasphemous.LostDreams.Localize("gfname");

    protected override string Description => Main.Blasphemous.LostDreams.Localize("gfdesc");

    protected override string Lore => Main.Blasphemous.LostDreams.Localize("gflore");

    protected override bool CarryOnStart => false;

    protected override bool PreserveInNGPlus => true;

    protected override void LoadImages(out Sprite picture)
    {
        picture = Main.Blasphemous.LostDreams.FileUtil.loadDataImages("guilt-fragment.png", new Vector2Int(30, 30), Vector2Int.zero, 32, 0, true, out Sprite[] images) ? images[0] : null;
    }
}

class GuiltFragmentEffect : ModItemEffectOnAcquire
{
    protected override bool ActivateOnce => false;

    protected override void ApplyEffect() => Main.Blasphemous.LostDreams.EffectHandler.Activate("guilt-fragment");

    protected override void RemoveEffect() => Main.Blasphemous.LostDreams.EffectHandler.Deactivate("guilt-fragment");
}
