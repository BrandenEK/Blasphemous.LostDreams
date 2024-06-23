using BepInEx;
using System;

namespace Blasphemous.LostDreams;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.1.0")]
[BepInDependency("Blasphemous.Framework.Items", "0.1.0")]
[BepInDependency("Blasphemous.Framework.Levels", "0.1.0")]
[BepInDependency("Blasphemous.Framework.Penitence", "0.2.0")]
internal class Main : BaseUnityPlugin
{
    public static LostDreams LostDreams { get; private set; }

    private void Start()
    {
        LostDreams = new LostDreams();
    }

    public static T Validate<T>(T obj, Func<T, bool> validate)
    {
        return validate(obj)
            ? obj
            : throw new Exception($"{obj} is an invalid import argument");
    }
}
