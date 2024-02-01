using Blasphemous.ModdingAPI;

namespace Blasphemous.LostDreams;

public class LostDreams : BlasMod
{
    public LostDreams() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnInitialize()
    {
        LogError($"{ModInfo.MOD_NAME} has been initialized");
    }
}
