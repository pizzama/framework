using UnityEngine;
using System;
using System.Collections.Generic;
using SFramework.Tools;
using Unity.VisualScripting;

namespace SFramework.Game
{
    public class RootControl : SControl
    {
        //
        private readonly Dictionary<int, STimeData> _countDownTimerDict =
            new Dictionary<int, STimeData>();
        private int _currentCountDownId = 0;

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

        public void CloseAllTimer()
        {
            foreach (var item in _countDownTimerDict)
            {
                item.Value.Stop();
            }

            _countDownTimerDict.Clear();
        } 

        protected override void controlUpdate()
        {
            foreach (var item in _countDownTimerDict)
            {
                item.Value.Update((int)Time.deltaTime*1000);
            }
        }

        protected override void closing()
        {
            base.closing();
            CloseAllTimer();
        }

        
    }
}
