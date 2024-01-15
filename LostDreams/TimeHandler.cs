using Framework.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostDreams;

internal class TimeHandler
{
    private readonly Dictionary<string, Timer> _timers = new();

    public void AddTimer(string id, float length, bool repeat, Action callback)
    {
        if (_timers.ContainsKey(id))
        {
            Main.LostDreams.LogError($"A timer with id {id} already exists.");
            return;
        }

        Main.LostDreams.Log($"Adding timer: {id}");
        _timers.Add(id, new Timer(length, repeat, callback));
    }

    public void RemoveTimer(string id)
    {
        Main.LostDreams.Log($"Removing timer: {id}");
        _timers.Remove(id);
    }

    public void Update()
    {
        if (Core.Logic.Penitent == null)
            return;

        foreach (var timer in _timers.Values)
        {
            timer.OnUpdate();
        }
    }

    public void Reset()
    {
        Main.LostDreams.Log("Clearing all timers");
        _timers.Clear();
    }

    class Timer
    {
        private readonly Action _callback;
        private readonly float _length;
        private readonly bool _repeat;

        private float _nextActivation;

        public Timer(float length, bool repeat, Action callback)
        {
            _length = length;
            _repeat = repeat;
            _callback = callback;

            _nextActivation = Time.time + _length;
        }

        // Maybe return true if repeat is true so that the timer handler can remove it
        public bool OnUpdate()
        {
            if (Time.time < _nextActivation)
                return false;

            _nextActivation = Time.time + _length;
            _callback();
            return true;
        }
    }
}
