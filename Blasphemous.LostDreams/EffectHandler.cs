using System.Collections.Generic;

namespace Blasphemous.LostDreams;

internal class EffectHandler
{
    private readonly List<string> _activeEffects = new();

    public void Activate(string effect)
    {
        if (!_activeEffects.Contains(effect))
        {
            Main.LostDreams.Log("Activating effect: " + effect);
            _activeEffects.Add(effect);
        }
    }

    public void Deactivate(string effect)
    {
        Main.LostDreams.Log("Deactivating effect: " + effect);
        _activeEffects.Remove(effect);
    }

    public void Reset()
    {
        Main.LostDreams.Log("Clearing all item effects");
        _activeEffects.Clear();
    }

    public bool IsActive(string effect)
    {
        return _activeEffects.Contains(effect);
    }
}
