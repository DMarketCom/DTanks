using SHLibrary.StateMachine;

namespace DevInstruments.DevConsole.States
{
    public abstract class DevConsoleStateBase : StateBase<DevConsoleController, DevConsoleView>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            CreateButtons();
            View.Clicked += OnButtonClicked;
        }
        
        public override void Finish()
        {
            base.Finish();
            View.DestroyAllButtons();
            View.Clicked -= OnButtonClicked;
        }

        protected abstract void CreateButtons();
        protected abstract void OnButtonClicked(int value);
    }
}
