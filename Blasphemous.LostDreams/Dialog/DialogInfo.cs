using Framework.Dialog;

namespace Blasphemous.LostDreams.Dialog;

public class DialogInfo
{
    public string Id { get; }
    public DialogObject.DialogType Type { get; }
    public string Item { get; }

    public DialogInfo(string id, DialogObject.DialogType type, string item)
    {
        Id = Main.Validate(id, x => !string.IsNullOrEmpty(x));
        Type = type;
        Item = Main.Validate(item, x => type != DialogObject.DialogType.GiveObject && type != DialogObject.DialogType.BuyObject || !string.IsNullOrEmpty(x));
    }
}
