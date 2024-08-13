using Blasphemous.ModdingAPI.Localization;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.LostDreams.Extensions;

internal static class LocalizationExtensions
{
    public static List<string> LocalizeLines(this LocalizationHandler handler, string key)
    {
        string localized = handler.Localize(key);
        return localized.Split('@').Select(x => x.Trim()).ToList();
    }
}
