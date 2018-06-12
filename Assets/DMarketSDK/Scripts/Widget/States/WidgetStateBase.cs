using DMarketSDK.Common.ErrorHelper;
using SHLibrary.StateMachine;

namespace DMarketSDK.Widget.States
{
    public class WidgetStateBase : StateBase<Widget, WidgetView>
    {
        protected WidgetModel Model { get { return Controller.Model;  } }

        protected IWidgetErrorHelper WidgetErrorHelper
        { get { return Controller.WidgetErrorHelper; } }

        protected IApiErrorHelper ApiErrorHelper
        { get { return Controller.ApiErrorHelper; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.ShowClicked += OnShowWidgetClicked;
            View.HideClicked += OnHideWidgetClicked;
        }

        public override void Finish()
        {
            base.Finish();
            View.ShowClicked -= OnShowWidgetClicked;
            View.HideClicked -= OnHideWidgetClicked;
        }

        private void OnShowWidgetClicked()
        {
            (Controller as IWidget).Open();
        }

        private void OnHideWidgetClicked()
        {
            (Controller as IWidget).Close();
        }
    }
}