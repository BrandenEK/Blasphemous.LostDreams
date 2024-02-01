using Blasphemous.LostDreams.Acquisition;
using Blasphemous.LostDreams.Events;
using Blasphemous.LostDreams.Items.ChargeTime;
using Blasphemous.LostDreams.Items.DamageRemoval;
using Blasphemous.LostDreams.Items.DamageStack;
using Blasphemous.LostDreams.Items.GuiltFragment;
using Blasphemous.LostDreams.Items.HealthRegen;
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Items;

namespace Blasphemous.LostDreams;

public class LostDreams : BlasMod
{
    public LostDreams() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    internal AcquisitionHandler AcquisitionHandler { get; } = new();
    internal EffectHandler EffectHandler { get; } = new();
    internal EventHandler EventHandler { get; } = new();
    internal TimeHandler TimeHandler { get; } = new();

    protected override void OnAllInitialized()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
    }

    protected override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel != "MainMenu")
            return;

        // Reset handlers when exiting a game
        EffectHandler.Reset();
        AcquisitionHandler.Reset();
        EventHandler.Reset();
        TimeHandler.Reset();
    }

    protected override void OnUpdate()
    {
        // Update handlers every frame
        TimeHandler.Update();
    }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        // Beads
        provider.RegisterItem(new ChargeTimeBead().AddEffect(new ChargeTimeEffect())); // RB501
        provider.RegisterItem(new DamageStackBead().AddEffect(new DamageStackEffect())); // RB502
        provider.RegisterItem(new DamageRemovalBead().AddEffect(new DamageRemovalEffect())); // RB503

        // Sword hearts
        provider.RegisterItem(new HealthRegenHeart().AddEffect(new HealthRegenEffect())); // HE501

        // Quest items
        provider.RegisterItem(new GuiltFragmentItem().AddEffect(new GuiltFragmentEffect())); // QI502
    }
}
