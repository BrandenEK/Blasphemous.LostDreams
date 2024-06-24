using Blasphemous.LostDreams.Acquisition;
using Blasphemous.LostDreams.Animation;
using Blasphemous.LostDreams.Dialog;
using Blasphemous.LostDreams.Effects;
using Blasphemous.LostDreams.Events;
using Blasphemous.LostDreams.Items;
using Blasphemous.LostDreams.Items.QuestItems;
using Blasphemous.LostDreams.Items.RosaryBeads;
using Blasphemous.LostDreams.Items.SwordHearts;
using Blasphemous.LostDreams.Levels;
using Blasphemous.LostDreams.Npc;
using Blasphemous.LostDreams.Penitences;
using Blasphemous.LostDreams.Timing;
using Blasphemous.ModdingAPI;
using Blasphemous.Framework.Items;
using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.Framework.Penitence;
using UnityEngine;

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

    // Item lists
    internal QuestItemList QuestItemList { get; private set; }
    internal RosaryBeadList RosaryBeadList { get; private set; }
    internal SwordHeartList SwordHeartList { get; private set; }

    // Info storages
    internal AnimationStorage AnimationStorage { get; private set; }
    internal DialogStorage DialogStorage { get; private set; }
    internal NpcStorage NpcStorage { get; private set; }

    // Special effects
    internal HealthDrain HealthDrain { get; private set; }

    /// <summary>
    /// Register handlers and create special effects
    /// </summary>
    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
        Config cfg = ConfigHandler.Load<Config>();
        ConfigHandler.Save(cfg);

        // Initialize item lists
        QuestItemList = new QuestItemList(cfg);
        RosaryBeadList = new RosaryBeadList(cfg);
        SwordHeartList = new SwordHeartList(cfg);

        // Initialize info storages
        AnimationStorage = new AnimationStorage(FileHandler);
        DialogStorage = new DialogStorage(FileHandler);
        NpcStorage = new NpcStorage(FileHandler);

        HealthDrain = new HealthDrain(cfg.PE501);
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
        foreach (var quest in QuestItemList.Items)
            provider.RegisterItem(quest);

        foreach (var bead in RosaryBeadList.Items)
            provider.RegisterItem(bead);

        foreach (var heart in SwordHeartList.Items)
            provider.RegisterItem(heart);

        provider.RegisterItem(new StandardRosaryBead("RB551", true));

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
        provider.RegisterObjectCreator("npc", new ObjectCreator(
            new NpcLoader(),
            new NpcModifier()));
    }
}
