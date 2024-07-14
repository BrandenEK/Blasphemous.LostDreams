using Blasphemous.ModdingAPI.Files;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LostDreams.Animation;

public class AnimationStorage
{
    private readonly Dictionary<string, AnimationInfo> _animations = new();

    public AnimationInfo this[string name] => _animations.TryGetValue(name, out var info)
        ? info : throw new System.Exception($"Animation {name} was never loaded");

    public AnimationStorage(FileHandler file)
    {
        string infoPath = "animations.json";
        if (!file.LoadDataAsJson(infoPath, out AnimationImportInfo[] imports))
        {
            Main.LostDreams.LogError("Failed to load animation list");
            return;
        }

        foreach (var import in imports)
        {
            var options = new SpriteImportOptions()
            {
                Pivot = import.Pivot
            };

            if (!file.LoadDataAsFixedSpritesheet(import.FilePath, import.Size, out Sprite[] spritesheet, options))
            {
                Main.LostDreams.LogError($"Failed to load {import.Name} from {import.FilePath}");
                continue;
            }

            _animations.Add(import.Name, new AnimationInfo(import.Name, spritesheet, import.SecondsPerFrame));
        }

        Main.LostDreams.Log($"Loaded {_animations.Count} animations");
    }
}
