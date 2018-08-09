using System;
using SHLibrary.ObserverView;

namespace SHLibrary.StateMachine
{
    public abstract class StateBase
    {
        private readonly string _name;
        private readonly TimerComponent _timer;

        protected StateBase()
        {
            _timer = new TimerComponent();
            _name = GetType().Name;
        }

        public event Action<Type, object[]> NeedApplyState;
        public event Action<object[]> NeedApplyPreviousState;
        public event Action<CommandBase> NeedApplyCommand;

        public virtual void Start(object[] args = null)
        {
            _timer.UpdatedTime += OnScheduledUpdate;
        }

        public virtual void Update()
        {
            _timer.UpdateTimerTime();
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Finish()
        {
            _timer.UpdatedTime -= OnScheduledUpdate;
        }

        public virtual void Binding(IStateMachine controller, ViewBase view)
        {
        }

        protected void ApplyState<T>(params object[] args)
            where T : StateBase
        {
            NeedApplyState.SafeRaise(typeof(T), args);
        }

        protected void ApplyPreviousState(params object[] args)
        {
            NeedApplyPreviousState.SafeRaise(args);
        }

        protected void ApplyCommand(CommandBase command)
        {
            NeedApplyCommand.SafeRaise(command);
        }

        protected void ScheduledUpdate(float time, bool repeat = false)
        {
            _timer.RunTimer(time, repeat);
        }

        protected void StopScheduled()
        {
            _timer.Stop();
        }

        protected virtual void OnScheduledUpdate()
        {
        }

        protected virtual void OnBackClick()
        {
        }

        public override string ToString()
        {
            return _name;
        }
    }

    public abstract class StateBase<TController> : StateBase
        where TController : IStateMachine
    {
        protected TController Controller { private set; get; }

        public override void Binding(IStateMachine controller, ViewBase view)
        {
            base.Binding(controller, view);
            Controller = (TController)controller;
        }
    }

    public abstract class StateBase<TController, TView> : StateBase<TController>
        where TController : StateMachineBase
        where TView : ViewBase
    {
        protected TView View { private set; get; }

        public override void Binding(IStateMachine controller, ViewBase view)
        {
            base.Binding(controller, view);
            View = view as TView;
        }
    }
}
