using System.Collections;
using UnityEngine;
using SFramework.Game;
using System;
using App.Static;
using UnityEngine.Rendering.Universal;
namespace App.Farm
{
    [RequireComponent(typeof(Light2D))]
    public class WorldTime : RootEntity
    {
        public const int MinutesInDay = 1440;
        private float _dayLength;
        private float _minuteLength => _dayLength / MinutesInDay;
        private TimeSpan _currentTime;

        public Action<TimeSpan> RefreshTimeSpan;
        [SerializeField] private Light2D _light;
        [SerializeField] private Gradient _gradient;
        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(1);

            RefreshTimeSpan?.Invoke(_currentTime);
            _light.color = _gradient.Evaluate(percentOfDay(_currentTime));

            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
        }

        protected override void initEntity()
        {
            _light = GetComponent<Light2D>();
        }

        public override void DestroyEntity()
        {
        }

        public override void Recycle()
        {
        }

        public override void Show()
        {
            StartCoroutine(AddMinute());
        }

        private float percentOfDay(TimeSpan timeSpan)
        {
            return (float)timeSpan.TotalMinutes % MinutesInDay / MinutesInDay;
        }
    }
}
