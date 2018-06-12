using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public class WidgetConnectionLostState : WidgetFormStateBase<ConnectionLostForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.BtnRetry.onClick.AddListener(OnRetryClick);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.BtnRetry.onClick.RemoveListener(OnRetryClick);
        }

        private void OnRetryClick()
        {
            ApplyPreviousState();
        }
    }
}