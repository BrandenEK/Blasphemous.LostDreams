using Blasphemous.ModdingAPI.Files;
using System.Collections.Generic;

namespace Blasphemous.LostDreams.Dialog;

public class DialogStorage
{
    private readonly Dictionary<string, DialogInfo> _dialogs = new();

    public DialogInfo this[string id] => _dialogs.TryGetValue(id, out var info)
        ? info : throw new System.Exception($"Dialog {id} was never loaded");

    public DialogStorage(FileHandler file)
    {
        string infoPath = "dialogs.json";
        if (!file.LoadDataAsJson(infoPath, out DialogInfo[] imports))
        {
            Main.LostDreams.LogError("Failed to load dialog list");
            return;
        }

        foreach (var import in imports)
        {
            _dialogs.Add(import.Id, import);
        }

        Main.LostDreams.Log($"Loaded {_dialogs.Count} dialogs");
    }
}
