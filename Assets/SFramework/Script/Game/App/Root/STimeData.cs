using System;
using SFramework.Tools;

namespace SFramework.Game
{
    public sealed class STimeData
    {
        public enum TimeState
        {
            Play,
            Pause,
            Stop
        };

        public int Tid => _tid;

        private int _tid;
        private long _startTimeStamp;
        private long _endTimeStamp;
        private bool _isLoop;
        private TimeState _state;
        private Action<int, object> _timeEndCallBack;

        public STimeData(
            int tid,
            int coolDown,
            bool isLoop,
            Action<int, object> timeEndCallBack
        )
        {
            _tid = tid;
            _startTimeStamp = TimeTools.GetTimeStampSecond();
            _endTimeStamp = _startTimeStamp + coolDown;
            _isLoop = isLoop;
            _timeEndCallBack = timeEndCallBack;
            _state = TimeState.Stop;
        }

        public void TriggerTimeEndCallBack()
        {
            _timeEndCallBack?.Invoke(_tid, null);
        }

        public void Reset()
        {
            if (_state == TimeState.Stop)
            {
                return;
            }
            long passTime = _endTimeStamp - _startTimeStamp;
            _startTimeStamp = TimeTools.GetTimeStampSecond();
            _endTimeStamp = _startTimeStamp + passTime; 
        }

        public void Pause()
        {
            _state = TimeState.Pause;
        }

        public void Play()
        {
            _state = TimeState.Play;
        }

        public void Stop()
        {
            _state = TimeState.Stop;
        }

        public void Update(int timePass)
        {
            
        }
    }
}
