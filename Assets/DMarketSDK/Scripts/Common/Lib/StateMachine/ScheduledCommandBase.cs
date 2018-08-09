using UnityEngine;

namespace SHLibrary.StateMachine
{
    public abstract class ScheduledCommandBase : CommandBase
    {
        private readonly TimerComponent _timer = new TimerComponent();
        private float _timeOfCommandStart;

        protected float TimeSinceCommandStart
        {
            get { return Time.timeSinceLevelLoad - _timeOfCommandStart; }
        }

        public override void Start()
        {
            base.Start();
            _timer.UpdatedTime += OnScheduledUpdate;
            _timeOfCommandStart = Time.timeSinceLevelLoad;
        }

        public override void Update()
        {
            base.Update();
            _timer.UpdateTimerTime();
        }

        protected override void Finish()
        {
            base.Finish();
            _timer.UpdatedTime -= OnScheduledUpdate;
            StopScheduled();
        }

        protected void ScheduledUpdate(float time, bool repeat = false)
        {
            _timer.RunTimer(time, repeat);
        }

        protected virtual void OnScheduledUpdate()
        {
        }

        protected void StopScheduled()
        {
            _timer.Stop();
        }
    }

    public abstract class ScheduledCommandBase<T> : ScheduledCommandBase
        where T : StateMachineBase
    {
        protected T Controller
        {
            get { return StateMachine as T; }
        }
    }
}
