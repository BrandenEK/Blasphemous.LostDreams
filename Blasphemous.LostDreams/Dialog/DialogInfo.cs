using Framework.Dialog;

namespace Blasphemous.LostDreams.Dialog;

public class DialogInfo
{
    public string Id { get; }
    public DialogObject.DialogType Type { get; }
    public string[] TextLines { get; }
    public string[] ResponseLines { get; }
    public string Item { get; }

    public DialogInfo(string id, DialogObject.DialogType type, string[] textLines, string[] responseLines, string item)
    {
        Id = Main.Validate(id, x => !string.IsNullOrEmpty(x));
        Type = type;
        TextLines = Main.Validate(textLines, x => x != null && x.Length > 0);
        ResponseLines = responseLines ?? new string[0];
        Item = Main.Validate(item, x => type != DialogObject.DialogType.GiveObject && type != DialogObject.DialogType.BuyObject || !string.IsNullOrEmpty(x));
    }
}
