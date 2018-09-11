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

        protected TForm GetForm<TForm>() where TForm : WidgetFormViewBase
        {
            return Controller.GetForm<TForm>();
        }

        protected void ShowSimpleNotification(string message)
        {
            Controller.SoundManager.Play(MarketSoundType.SimpleMessage);
            Controller.PopUpController.ShowSimpleNotification(message);
        }

        private void OnLogoutClicked()
        {
            GetForm<ApproveForm>().ShowChoiceWindow("Are you sure?", OnLogoutResult);  
        }

        private void OnCloseMarketClicked()
        {
            Controller.Close();
        }

        private void OnLogoutResult(bool isLogout)
        {
            if (isLogout)
            {
                Controller.Logout();
            }
        }
    }
}