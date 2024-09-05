using Blasphemous.LostDreams.Acquisition;
using Blasphemous.LostDreams.Animation;
using Blasphemous.LostDreams.Dialog;
using Blasphemous.LostDreams.Events;
using Blasphemous.LostDreams.Items.Penitences;
using Blasphemous.LostDreams.Items.QuestItems;
using Blasphemous.LostDreams.Items.RosaryBeads;
using Blasphemous.LostDreams.Items.SwordHearts;
using Blasphemous.LostDreams.Levels;
using Blasphemous.LostDreams.Npc;
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
    internal EventHandler EventHandler { get; } = new();

    // Item lists
    internal PenitenceList PenitenceList { get; private set; }
    internal QuestItemList QuestItemList { get; private set; }
    internal RosaryBeadList RosaryBeadList { get; private set; }
    internal SwordHeartList SwordHeartList { get; private set; }

    // Info storages
    internal AnimationStorage AnimationStorage { get; private set; }
    internal DialogStorage DialogStorage { get; private set; }
    internal NpcStorage NpcStorage { get; private set; }

    /// <summary>
    /// Register handlers and create special effects
    /// </summary>
    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
        Config cfg = ConfigHandler.Load<Config>();
        ConfigHandler.Save(cfg);

        // Initialize item lists
        PenitenceList = new PenitenceList(cfg);
        QuestItemList = new QuestItemList(cfg);
        RosaryBeadList = new RosaryBeadList(cfg);
        SwordHeartList = new SwordHeartList(cfg);

        // Initialize info storages
        AnimationStorage = new AnimationStorage(FileHandler);
        DialogStorage = new DialogStorage(FileHandler);
        NpcStorage = new NpcStorage(FileHandler);
    }

    /// <summary>
    /// Reset handlers when exiting a game
    /// </summary>
    protected override void OnExitGame()
    {
        AcquisitionHandler.Reset();
        EventHandler.Reset();
    }

    /// <summary>
    /// Update handlers every frame
    /// </summary>
    protected override void OnUpdate()
    {
        if (!LoadStatus.GameSceneLoaded)
            return;

        // Temporarily update penitences until handled by framework
        PenitenceList.PE501.Update();
    }

    /// <summary>
    /// Register all custom things
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        foreach (var penitence in PenitenceList.Items)
            provider.RegisterPenitence(penitence);

        foreach (var quest in QuestItemList.Items)
            provider.RegisterItem(quest);

        foreach (var bead in RosaryBeadList.Items)
            provider.RegisterItem(bead);

        foreach (var heart in SwordHeartList.Items)
            provider.RegisterItem(heart);

        // Level edits
        provider.RegisterObjectCreator("patio-column", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Arcs/garden-spritesheet_31 (3)"),
            new NoModifier("Column")));
        provider.RegisterObjectCreator("patio-bricks", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Arcs/garden-spritesheet_41 (6)"),
            new NoModifier("Bricks")));
        provider.RegisterObjectCreator("npc", new ObjectCreator(
            new NpcLoader(),
            new NpcModifier()));
        provider.RegisterObjectCreator("door", new ObjectCreator(
            new SceneLoader("D17Z01S10_LOGIC", "DOORS/{0}"),
            new DoorModifier()));
        provider.RegisterObjectCreator("wasteland-stone-diagonal", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/Walls/churches-field-spritesheet-improved_20"),
            new NoModifier("Diagonal Stone")));
        provider.RegisterObjectCreator("wasteland-stone-vertical", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/Walls/churches-field-spritesheet-improved_25"),
            new NoModifier("Vertical Stone")));
        provider.RegisterObjectCreator("wasteland-tree", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/Props/Trees/churches-field-spritesheet-improved_41"),
            new NoModifier("Wasteland Tree")));
        provider.RegisterObjectCreator("wasteland-rock_1", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/Props/Rocks/churches-field-spritesheet-improved_14"),
            new NoModifier("Wasteland Rock 1")));
        provider.RegisterObjectCreator("wasteland-rock_2", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/Props/Rocks/churches-field-spritesheet-improved_48"),
            new NoModifier("Wasteland Rock 2")));
        provider.RegisterObjectCreator("wasteland-grass_1", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/AnimatedProps/MovingGrassChurchesField_1"),
            new NoModifier("Grass 1")));
        provider.RegisterObjectCreator("wasteland-grass_2", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/AnimatedProps/churches-field-spritesheet-improved_37"),
            new NoModifier("Grass 2")));
        provider.RegisterObjectCreator("wasteland-grass_3", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/AnimatedProps/MovingGrassChurchesField_2"),
            new NoModifier("Grass 3")));

        provider.RegisterObjectCreator("patio-floor", new ObjectCreator(
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/garden-spritesheet_13 (2)"),
            new ColliderModifier("Floor",
                                 new Vector2(2.9f, 0.9f),
                                 new Vector2(0, -0.3f))));
        provider.RegisterObjectCreator("wasteland-platform-rock", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/Floor/churches-field-spritesheet-improved_0"),
            new ColliderModifier("OneWayDown",
                                 new Vector2(4f, 1.4f),
                                 new Vector2(0, -0.3f))));
        provider.RegisterObjectCreator("wasteland-wall", new ObjectCreator(
            new SceneLoader("D01Z03S06_DECO", "MIDDLEGROUND/AfterPlayer/SideDoor/churches-field-spritesheet-improved_73"),
            new ColliderModifier("Floor",
                                 new Vector2(2f, 2f))));
    }
}
