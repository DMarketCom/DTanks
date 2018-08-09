using DMarketSDK.Basic.Forms;
using DMarketSDK.Forms;

namespace DMarketSDK.Basic.States
{
    public sealed class BasicWidgetLoggedFormState : BasicWidgetFormStateBase<WidgetLoggedForm, WidgetLoginFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.BtnLogout.onClick.AddListener(OnLogoutClicked);
            View.BtnClose.onClick.AddListener(OnCloseMarketClicked);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnLogout.onClick.RemoveListener(OnLogoutClicked);
            View.BtnClose.onClick.RemoveListener(OnCloseMarketClicked);
        }
    }
}