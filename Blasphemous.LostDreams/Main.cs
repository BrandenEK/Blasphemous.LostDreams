using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.4.1")]
[BepInDependency("Blasphemous.Framework.Items", "0.1.1")]
[BepInDependency("Blasphemous.Framework.Levels", "0.1.4")]
[BepInDependency("Blasphemous.Framework.Penitence", "0.2.1")]
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

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        sVector = sVector.Trim().TrimStart('(').TrimEnd(')');

        // split the items
        List<string> stringList = sVector.Split(',').ToList();

        // convert to Vector3
        Vector3 result;
        while (stringList.Count < 3)
        {
            stringList.Add("0.0");
        }
        result = new Vector3(
            StringToFloatOrDefault(stringList[0]),
            StringToFloatOrDefault(stringList[1]),
            StringToFloatOrDefault(stringList[2]));
        return result;

        static float StringToFloatOrDefault(string input)
        {
            float result;
            try
            {
                result = float.Parse(input);
            }
            catch
            {
                result = 0f;
            }
            return result;
        }
    }
}
