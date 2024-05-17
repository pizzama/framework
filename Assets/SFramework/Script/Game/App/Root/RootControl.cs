using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework.Game
{
    public class RootControl : SControl
    {
        private readonly Dictionary<int, STimeData> _countDownTimerDict =
            new Dictionary<int, STimeData>();
        private int _currentCountDownId = 0;

        public int StartCountDownTimer(
            float countdown,
            float intervalTick,
            Action<int> timeEndCallBack,
            Action<int> intervalCallBack
        )
        {
            if (countdown <= 0)
                return 0;
            _currentCountDownId++;
            STimeData data;
            if (!_countDownTimerDict.TryGetValue(_currentCountDownId, out data))
            {
                data = new STimeData(_currentCountDownId, countdown, intervalTick, timeEndCallBack, intervalCallBack);
                _countDownTimerDict.Add(data.Tid, data);
            }

            if (data != null)
                return data.Tid;
            return 0;
        }

        public void CloseTimerByID(int tid)
        {
            if (_countDownTimerDict.ContainsKey(tid))
            {
                _countDownTimerDict[tid].Stop();
                _countDownTimerDict.Remove(tid);
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

        public override void FixUpdate()
        {
            base.FixUpdate();
            List<int> needRemoveKeys = new List<int>();
            foreach (var item in _countDownTimerDict)
            {
                item.Value.Update(Time.fixedUnscaledDeltaTime);
                if(!item.Value.IsAvailable())
                {
                    needRemoveKeys.Add(item.Key);
                }
            }

            foreach (var item in needRemoveKeys)
            {
                if(_countDownTimerDict.ContainsKey(item))
                {
                    _countDownTimerDict.Remove(item);
                }
            }
        }

        public override void Update()
        {
            base.Update();
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
