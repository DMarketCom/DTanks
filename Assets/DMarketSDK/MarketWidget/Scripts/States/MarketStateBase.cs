using DMarketSDK.Common.Sound;
using DMarketSDK.Forms;
using DMarketSDK.Market.Strategy;
using DMarketSDK.WidgetCore.Forms;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.States
{
    public abstract class MarketStateBase : StateBase<MarketWidgetController>
    {
        protected WidgetModel WidgetModel { get { return Controller.Model; } }

        protected MarketView MarketView { get { return Controller.View; } }

        protected IMarketStrategy Strategy { get { return Controller.Strategy; } }

        private ApproveForm ApproveForm { get { return Controller.GetForm<ApproveForm>(); } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            MarketView.LogoutClicked += OnLogoutClicked;
            MarketView.CloseClicked += OnCloseMarketClicked;
        }

        public override void Finish()
        {
            base.Finish();
            MarketView.LogoutClicked -= OnLogoutClicked;
            MarketView.CloseClicked -= OnCloseMarketClicked;
        }

        protected void ShowSimpleNotification(string message)
        {
            Controller.SoundManager.Play(MarketSoundType.SimpleMessage);
            Controller.PopUpController.ShowSimpleNotification(message);
        }

        private void OnLogoutClicked()
        {
            ApproveForm.ShowChoiceWindow("Are you sure?", DoLogout);  
        }

        private void DoLogout(bool result)
        {
            if (result)
            {
                Controller.Logout();
            }
        }

        private void OnCloseMarketClicked()
        {
            ApproveForm.ShowChoiceWindow("Are you sure?", DoClose);
        }

        private void DoClose(bool result)
        {
            if (result)
            {
                Controller.Close();
            }
        }
    }
}