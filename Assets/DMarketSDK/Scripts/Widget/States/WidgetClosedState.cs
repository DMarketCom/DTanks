namespace DMarketSDK.Widget.States
{
    public class WidgetClosedState : WidgetStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.HideWaitPanel();
            View.Hide();
        }
    }
}
