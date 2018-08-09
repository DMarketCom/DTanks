namespace DMarketSDK.Market.States
{
    public class MarketClosedState : MarketStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            Controller.ApplyScreenSettings(WidgetModel.GameOrientationSettings);
            MarketView.Hide();
        }
    }
}