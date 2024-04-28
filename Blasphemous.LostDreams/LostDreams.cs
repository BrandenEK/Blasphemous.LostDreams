using Blasphemous.LostDreams.Acquisition;
using Blasphemous.LostDreams.Effects;
using Blasphemous.LostDreams.Events;
using Blasphemous.LostDreams.Items;
using Blasphemous.LostDreams.Levels;
using Blasphemous.LostDreams.Penitences;
using Blasphemous.LostDreams.Timing;
using Blasphemous.ModdingAPI;
using Blasphemous.Framework.Items;
using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.Framework.Penitence;
using UnityEngine;
using Blasphemous.LostDreams.Swords;

namespace Blasphemous.LostDreams;

/// <summary>
/// Handles all new item and penitence effects
/// </summary>
public class LostDreams : BlasMod
{
    internal LostDreams() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    // Handlers
    internal AcquisitionHandler AcquisitionHandler { get; } = new();
    internal ItemHandler ItemHandler { get; } = new();
    internal EventHandler EventHandler { get; } = new();
    internal PenitenceHandler PenitenceHandler { get; } = new();
    internal TimeHandler TimeHandler { get; } = new();

    // Special effects
    internal IToggleEffect DamageRemoval { get; private set; }
    internal IMultiplierEffect DamageStack { get; private set; }
    internal IToggleEffect HE502 { get; private set; }
    internal HealthDrain HealthDrain { get; private set; }

    /// <summary>
    /// Register handlers and create special effects
    /// </summary>
    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
        Config cfg = ConfigHandler.Load<Config>();
        ConfigHandler.Save(cfg);

        DamageRemoval = new DamageRemoval();
        DamageStack = new DamageStack(cfg.RB502);
        HE502 = new HE502();
        HealthDrain = new HealthDrain(cfg.PE501);

        // Temp !!!
        _tempHE501 = cfg.HE501;
    }

    /// <summary>
    /// Reset handlers when exiting a game
    /// </summary>
    protected override void OnExitGame()
    {
        ItemHandler.Reset();
        AcquisitionHandler.Reset();
        EventHandler.Reset();
        TimeHandler.Reset();
    }

    /// <summary>
    /// Update handlers every frame
    /// </summary>
    protected override void OnUpdate()
    {
        TimeHandler.Update();
    }

    /// <summary>
    /// Register all custom things
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        // Beads
        provider.RegisterItem(new StandardRosaryBead("RB501", true));
        provider.RegisterItem(new StandardRosaryBead("RB502", true));
        provider.RegisterItem(new StandardRosaryBead("RB503", true));

        provider.RegisterItem(new StandardRosaryBead("RB551", true));

        // Sword hearts
        provider.RegisterItem(new StandardSwordHeart("HE501", false).AddEffect(new HealthRegen(_tempHE501)));
        provider.RegisterItem(new StandardSwordHeart("HE502", true));

        // Quest items
        provider.RegisterItem(new StandardQuestItem("QI502", false));

        // Penitences
        provider.RegisterPenitence(new StandardPenitence("PE501", "RB551"));

        // Level edits
        provider.RegisterObjectCreator("patio-column", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Arcs/garden-spritesheet_31 (3)"),
            new NoModifier("Column")));
        provider.RegisterObjectCreator("patio-bricks", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Arcs/garden-spritesheet_41 (6)"),
            new NoModifier("Bricks")));
        provider.RegisterObjectCreator("patio-floor", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/garden-spritesheet_13 (2)"),
            new ColliderModifier("Floor", new Vector2(2.7f, 0.4f))));
    }

    // Temp !!!
    private HE501Config _tempHE501;
}
