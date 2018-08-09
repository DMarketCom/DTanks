namespace DMarketSDK.Basic.States
{
    public class BasicWidgetClosedState : BasicWidgetStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            Controller.ApplyScreenSettings(WidgetModel.GameOrientationSettings);
            WidgetView.Hide();
        }
    }
}