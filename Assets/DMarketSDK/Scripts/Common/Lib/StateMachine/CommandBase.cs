using System;

namespace SHLibrary.StateMachine
{
    public abstract class CommandBase
    {
        private bool _isFinishedByTerminate;

        public IStateMachine StateMachine { get; set; }

        public bool Result { get; protected set; }

        public bool IsRunning { get; private set; }

        public virtual bool IsIndependentFromState
        {
            get { return true; }
        }

        public event Action<CommandBase> CommandFinished;
        public event Action<CommandBase> NeedApplyCommand;

        public virtual void Start()
        {
            Result = true;
            IsRunning = true;
            _isFinishedByTerminate = false;
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public void Terminate(bool? result = null)
        {
            if (IsRunning)
            {
                _isFinishedByTerminate = true;
                if (result != null)
                {
                    Result = (bool)result;
                }
                Finish();
                CommandFinished.SafeRaise(this);
            }
        }

        protected virtual void Finish()
        {
            if (!_isFinishedByTerminate)
            {
               // WolLogger.Error("Not use Finish(), use Terminate()", LogCategory.Common);
            }
            IsRunning = false;
        }

        protected void ApplyCommand(CommandBase command)
        {
            NeedApplyCommand.SafeRaise(command);
        }
    }
}
