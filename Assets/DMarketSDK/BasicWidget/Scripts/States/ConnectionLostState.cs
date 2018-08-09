using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Basic.States
{
    public class ConnectionLostState : BasicWidgetFormStateBase<ConnectionLostForm, WidgetFormModel>
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