using System;
using UnityEngine;

namespace Blasphemous.LostDreams.Timing;

internal partial class TimeHandler
{
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
