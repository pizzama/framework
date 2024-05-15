using UnityEngine;
using System;
using System.Collections.Generic;
using SFramework.Tools;

namespace SFramework.Game
{
    public class RootControl : SControl
    {
        private readonly Dictionary<int, STimeData> _countDownTimerDict =
            new Dictionary<int, STimeData>();
        private int _currentCountDownId = 0;

        public int StartCountDownTimer(int countdown, Action<int, object> callback)
        {
            if (countdown <= 0)
                return 0;
            _currentCountDownId++;
            STimeData data;
            if (!_countDownTimerDict.TryGetValue(_currentCountDownId, out data))
            {
                data = new STimeData(_currentCountDownId, countdown, false, callback);
                // _countDownTimerDict
            }

            return 0;
        }
    }
}
