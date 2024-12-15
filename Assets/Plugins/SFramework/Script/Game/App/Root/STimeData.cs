using System;
using System.Diagnostics;
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
        private long _currentTimeStamp;
        private float _intervalTick; //second
        private float _countTick; //second
        private float _coolDown; //execute time second
        private bool _isLoop;
        private TimeState _state;
        private Action<int> _timeEndCallBack;
        private Action<int> _intervalCallBack;

        public STimeData(
            int tid,
            float coolDown,
            float intervalTick,
            Action<int> timeEndCallBack,
            Action<int> intervalCallBack
        )
        {
            _tid = tid;
            _startTimeStamp = TimeTools.GetTimeStampMilliSecond();
            _currentTimeStamp = _startTimeStamp;
            _coolDown = coolDown;
            _endTimeStamp = _startTimeStamp + (long)_coolDown * 1000;
            _intervalTick = intervalTick;
            if(_coolDown < 0)
                _isLoop = true;
            else
                _isLoop = false;
            _timeEndCallBack = timeEndCallBack;
            _intervalCallBack = intervalCallBack;
            _state = TimeState.Pause;
        }

        public void Reset()
        {
            if (_state == TimeState.Stop)
            {
                return;
            }
            long passTime = _endTimeStamp - _startTimeStamp;
            _startTimeStamp = TimeTools.GetTimeStampMilliSecond();
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

        public bool IsAvailable()
        {
            if (_state != TimeState.Stop)
            {
                return true;
            }

            return false;
        }

        public void Update(float timePass)
        {
            if (_state == TimeState.Play)
            {
                _countTick += timePass;
                _currentTimeStamp += (long)(timePass * 1000);
                if (_countTick > _intervalTick)
                {
                    _countTick = 0;
                    _intervalCallBack?.Invoke(_tid);
                }
                if (_currentTimeStamp >= _endTimeStamp)
                {
                    if(_isLoop)
                    {
                        Reset();
                    }
                    else
                    {
                        _state = TimeState.Stop;
                    }
                    _timeEndCallBack?.Invoke(_tid);
                }
            }
        }
    }
}
