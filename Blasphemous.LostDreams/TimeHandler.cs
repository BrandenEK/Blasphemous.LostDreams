using Framework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams;

internal class TimeHandler
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

    class Countdown(float length, Action onFinish) : ITimeable
    {
        private readonly Action _onFinish = onFinish;
        private readonly float _finishTime = Time.time + length;

        public bool ShouldBeRemoved { get; private set; } = false;
        public bool IsPermanent => false;

        public void OnUpdate()
        {
            if (Time.time < _finishTime)
                return;

            ShouldBeRemoved = true;
            _onFinish();
        }
    }

    class Ticker(float length, bool permanent, Action onTick) : ITimeable
    {
        private readonly Action _onTick = onTick;
        private readonly float _length = length;

        private float _nextActivation = Time.time + length;

        public bool ShouldBeRemoved => false;
        public bool IsPermanent => permanent;

        public void OnUpdate()
        {
            if (Time.time < _nextActivation)
                return;

            _nextActivation = Time.time + _length;
            _onTick();
        }
    }

    interface ITimeable
    {
        bool ShouldBeRemoved { get; }
        bool IsPermanent { get; }

        void OnUpdate();
    }
}
