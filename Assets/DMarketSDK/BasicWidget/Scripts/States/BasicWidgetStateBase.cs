using DMarketSDK.Forms;
using DMarketSDK.WidgetCore.Forms;
using SHLibrary.StateMachine;

namespace DMarketSDK.Basic.States
{
    public abstract class BasicWidgetStateBase : StateBase<BasicWidgetController>
    {
        protected WidgetModel WidgetModel { get { return Controller.Model; } }

        protected BasicView WidgetView { get { return Controller.WidgetView; } }

        private ApproveForm ApproveForm { get { return Controller.GetForm<ApproveForm>(); } }

        protected void OnLogoutClicked()
        {
            ApproveForm.ShowChoiceWindow("Are you sure?", DoLogout);  
        }

        private void DoLogout(bool result)
        {
            if (result)
            {
                Controller.PreLogout();
            }
        }

        protected void OnCloseMarketClicked()
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