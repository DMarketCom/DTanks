using DMarketSDK.Domain;
using DMarketSDK.Forms;

namespace DMarketSDK.Market.Forms
{
    public sealed class VerifyAccountState : MarketFormStateBase<VerifyAccountForm, WidgetFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.BtnLogin.onClick.AddListener(OnLoginClicked);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnLogin.onClick.RemoveListener(OnLoginClicked);
        }

        private void OnLoginClicked()
        {
            ApplyState<MarketLoginFormState>();
        }
    }
}