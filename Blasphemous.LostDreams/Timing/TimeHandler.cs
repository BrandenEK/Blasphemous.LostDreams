using Framework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blasphemous.LostDreams.Timing;

internal partial class TimeHandler
{
    private readonly Dictionary<string, ITimeable> _timers = new();

    public void AddCountdown(string id, float length, Action onFinish)
    {
        AddTimeable(id, new Countdown(length, onFinish));
    }

    public void AddTicker(string id, float length, bool permanent, Action onTick)
    {
        AddTimeable(id, new Ticker(length, permanent, onTick));
    }

    private void AddTimeable(string id, ITimeable timeable)
    {
        if (_timers.ContainsKey(id))
        {
            Main.LostDreams.LogError($"A timer with id {id} already exists.");
            return;
        }
        Main.LostDreams.Log($"Adding timer: {id}");
        _timers.Add(id, timeable);
    }

    public void RemoveTimer(string id)
    {
        Main.LostDreams.Log($"Removing timer: {id}");
        _timers.Remove(id);
    }

    private void RemoveTimeables(Func<ITimeable, bool> predicate)
    {
        var toRemove = _timers.Where(x => predicate(x.Value)).ToArray();
        foreach (var t in toRemove)
        {
            Main.LostDreams.Log($"Stopping timer: {t.Key}");
            _timers.Remove(t.Key);
        }
    }

    public void Update()
    {
        if (!Main.LostDreams.LoadStatus.GameSceneLoaded || Core.Logic.Penitent == null)
            return;

        // Update each timer
        foreach (var timer in _timers.Values)
            timer.OnUpdate();

        // Remove all unecessary timers
        RemoveTimeables(x => x.ShouldBeRemoved);
    }

    public void Reset()
    {
        RemoveTimeables(x => !x.IsPermanent);
    }
}
