namespace DMarketSDK.Widget.States
{
    public class WidgetLogoutState : WidgetStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            Model.LoggedUsername = null;
            Model.MarketAccessToken = null;
            Model.SetChanges();

            ApplyState<WidgetLoginState>();
        }
    }
}