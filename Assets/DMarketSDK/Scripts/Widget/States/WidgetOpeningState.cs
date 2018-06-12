namespace DMarketSDK.Widget.States
{
    public class WidgetOpeningState : WidgetStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.Show();
            View.HideWaitPanel();
            if (Model.IsLogin)
            {
                ApplyState<WidgetLoggedState>();
            }
            else
            {
                ApplyState<WidgetLoginState>();
            }
        }
    }
}