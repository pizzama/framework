using UnityEngine;
using System;
using System.Collections.Generic;

namespace SFramework.Game
{
    public class RootControl : SControl
    {
        private long _timeStamp;
        private readonly Dictionary<int, STimeData> _countDownTimerDict =
            new Dictionary<int, STimeData>();
        private int _currentCountDownId = 0;

        public override void Open(SBundleParams value)
        {
            base.Open(value);
            _timeStamp = DateTime.Now.Ticks;
        }
        

        public int StartCountDownTimer(int countdown, Action<int, object> callback, bool isLoop = false)
        {
            if (countdown <= 0)
                return 0;
            _currentCountDownId++;
            STimeData data;
            if (!_countDownTimerDict.TryGetValue(_currentCountDownId, out data))
            {
                data = new STimeData(_currentCountDownId, countdown, isLoop, callback);
                _countDownTimerDict.Add(data.Tid, data);
            }

            if(data != null)
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
            _timeStamp += ToTicks(Time.fixedUnscaledDeltaTime);
            base.FixUpdate();
        }

        public override void Update()
        {
            base.Update();
            foreach (var item in _countDownTimerDict)
            {
                item.Value.Update(_timeStamp);
            }
        }

        protected override void closing()
        {
            base.closing();
            CloseAllTimer();
        }

        private static long ToTicks(float second)
        {
            return (long) (second * 1000 * 10000);
        }

        
    }
}
