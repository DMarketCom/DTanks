using System;
using UnityEngine;

namespace SHLibrary.StateMachine
{
    internal sealed class TimerComponent
    {
        private bool _isNeedSchedule;
        private bool _isPause;
        private bool _isRepeat;
        private float _timeForSchedule;
        private float _timeScheduleInterval;

        public event Action UpdatedTime;

        public void UpdateTimerTime()
        {
            if (_isNeedSchedule)
            {
                if (_isPause)
                {
                    _timeForSchedule += Time.deltaTime;
                }
                else if (Time.timeSinceLevelLoad >= _timeForSchedule)
                {
                    _isNeedSchedule = _isRepeat;
                    UpdatedTime.SafeRaise();
                    if (_isRepeat)
                    {
                        RunTimer(_timeScheduleInterval, _isRepeat);
                    }
                }
            }
        }

        public void Pause()
        {
            _isPause = true;
        }

        public void Continue()
        {
            _isPause = false;
        }

        public void RunTimer(float time, bool repeat = false)
        {
            _isNeedSchedule = true;
            _timeScheduleInterval = time;
            _isRepeat = repeat;
            _timeForSchedule = Time.timeSinceLevelLoad + _timeScheduleInterval;
        }

        public void Stop()
        {
            _isNeedSchedule = false;
        }
    }
}
