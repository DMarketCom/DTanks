using DMarketSDK.Basic.Forms;

namespace DMarketSDK.Basic.States
{
    public sealed class BasicWidgetLoggedFormState : BasicWidgetFormStateBase<WidgetLoggedForm, WidgetLoggedFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.BtnLogout.onClick.AddListener(OnLogoutClicked);
            View.BtnClose.onClick.AddListener(OnCloseMarketClicked);

            SetUserName();
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnLogout.onClick.RemoveListener(OnLogoutClicked);
            View.BtnClose.onClick.RemoveListener(OnCloseMarketClicked);
        }

        private void SetUserName()
        {
            FormModel.UserName = WidgetModel.UserName;
            FormModel.SetChanges();
        }
    }
}