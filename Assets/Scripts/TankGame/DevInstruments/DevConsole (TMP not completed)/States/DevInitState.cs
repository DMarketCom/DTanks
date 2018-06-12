using System;

namespace DevInstruments.DevConsole.States
{
    public class DevAutorizationState : DevConsoleStateBase
    {
        protected override void CreateButtons()
        {
            foreach (var value in Enum.GetValues(typeof(DevConsoleUserType)))
            {
                View.CreateButton(value.ToString());
            }
        }

        protected override void OnButtonClicked(int value)
        {
        }
    }
}
