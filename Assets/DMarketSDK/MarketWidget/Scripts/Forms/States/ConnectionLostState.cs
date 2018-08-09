using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Market.Forms
{
    public class ConnectionLostState : MarketFormStateBase<ConnectionLostForm, WidgetFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.BtnRetry.onClick.AddListener(OnRetryClick);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnRetry.onClick.RemoveListener(OnRetryClick);
        }

        private void OnRetryClick()
        {
            ApplyPreviousState();
        }
    }
}