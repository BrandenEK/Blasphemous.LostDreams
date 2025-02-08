using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Files;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Blasphemous.LostDreams.Importing.Sprites;

/// <summary>
/// Stores all sprites that have been imported
/// </summary>
public class SpriteStorage
{
    private readonly Dictionary<string, Sprite> _sprites;

    /// <summary>
    /// Gets a sprite, if it was loaded
    /// </summary>
    public Sprite this[string name] => _sprites.TryGetValue(name, out Sprite sprite)
        ? sprite : throw new System.Exception($"Sprite {name} was never loaded");

    /// <summary>
    /// Imports all sprites specified in the json file
    /// </summary>
    public SpriteStorage(FileHandler file)
    {
        if (!file.LoadDataAsJson("sprites.json", out SpriteImportInfo[] infos))
        {
            ModLog.Error("Failed to load sprites list");
            return;
        }

        foreach (var info in infos)
        {
            ModLog.Info("Importing sprite " + info.Name);

            var options = new SpriteImportOptions()
            {
                Pivot = new Vector2(info.Pivot.X, info.Pivot.Y),
                PixelsPerUnit = info.PixelsPerUnit,
            };

            if (!file.LoadDataAsSprite(Path.Combine("sprites", $"{info.Name}.png"), out Sprite sprite, options))
            {
                ModLog.Error($"Failed to load sprite {info.Name}");
                continue;
            }

            _sprites.Add(info.Name, sprite);
        }

        ModLog.Info($"Loaded {_sprites.Count} sprites");
    }
}
