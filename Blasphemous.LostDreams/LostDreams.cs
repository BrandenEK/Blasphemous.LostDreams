using Blasphemous.LostDreams.Acquisition;
using Blasphemous.LostDreams.Effects;
using Blasphemous.LostDreams.Events;
using Blasphemous.LostDreams.Items;
using Blasphemous.LostDreams.Levels;
using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Items;
using Blasphemous.ModdingAPI.Levels;
using Blasphemous.ModdingAPI.Levels.Loaders;
using Blasphemous.ModdingAPI.Levels.Modifiers;
using UnityEngine;

namespace Blasphemous.LostDreams;

public class LostDreams : BlasMod
{
    public LostDreams() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    // Handlers
    internal AcquisitionHandler AcquisitionHandler { get; } = new();
    internal ItemHandler ItemHandler { get; } = new();
    internal EventHandler EventHandler { get; } = new();
    internal TimeHandler TimeHandler { get; } = new();

    // Special effects
    internal IToggleEffect DamageRemoval { get; } = new DamageRemoval();
    internal IMultiplierEffect DamageStack { get; } = new DamageStack();

    protected override void OnAllInitialized()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
    }

    protected override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        if (newLevel != "MainMenu")
            return;

        // Reset handlers when exiting a game
        ItemHandler.Reset();
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
        provider.RegisterItem(new StandardRosaryBead("RB501"));
        provider.RegisterItem(new StandardRosaryBead("RB502"));
        provider.RegisterItem(new StandardRosaryBead("RB503"));

        // Sword hearts
        provider.RegisterItem(new StandardSwordHeart("HE501"));

        // Quest items
        provider.RegisterItem(new StandardQuestItem("QI502", false));

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
}
