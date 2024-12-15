using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game
{
    public abstract class RootControl : SControl
    {
        private readonly Dictionary<int, STimeData> _countDownTimerDict =
            new Dictionary<int, STimeData>();
        private int _currentCountDownId = 0;

        public int StartCountDownTimer(
            float cooldown,
            float intervalTick,
            Action<int> timeEndCallBack,
            Action<int> intervalCallBack
        )
        {
            _currentCountDownId++;
            STimeData data;
            if (!_countDownTimerDict.TryGetValue(_currentCountDownId, out data))
            {
                data = new STimeData(_currentCountDownId, cooldown, intervalTick, timeEndCallBack, intervalCallBack);
                _countDownTimerDict.Add(data.Tid, data);
            }

            if (data != null)
            {
                data.Play();
                return data.Tid;
            }
            return 0;
        }

        public void CloseTimerByID(int tid)
        {
            if (_countDownTimerDict.ContainsKey(tid))
            {
                _countDownTimerDict[tid].Stop();
            }
        }

        public void CloseAllTimer()
        {
            foreach (var item in _countDownTimerDict)
            {
                item.Value.Stop();
            }
            _countDownTimerDict.Clear();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            List<int> needRemoveKeys = new List<int>();
            foreach (var item in _countDownTimerDict)
            {
                item.Value.Update(Time.fixedUnscaledDeltaTime);
                if (!item.Value.IsAvailable())
                {
                    needRemoveKeys.Add(item.Key);
                }
            }

            foreach (var item in needRemoveKeys)
            {
                if (_countDownTimerDict.ContainsKey(item))
                {
                    _countDownTimerDict.Remove(item);
                }
            }
        }

        protected override void closing()
        {
            base.closing();
            CloseAllTimer();
        }

        private static long ToTicks(float second)
        {
            return (long)(second * 1000 * 10000);
        }
    }
}
