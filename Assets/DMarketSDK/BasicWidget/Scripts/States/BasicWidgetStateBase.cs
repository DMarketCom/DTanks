using DMarketSDK.Forms;
using DMarketSDK.WidgetCore.Forms;
using SHLibrary.StateMachine;

namespace DMarketSDK.Basic.States
{
    public abstract class BasicWidgetStateBase : StateBase<BasicWidgetController>
    {
        protected WidgetModel WidgetModel { get { return Controller.Model; } }

        protected BasicView WidgetView { get { return Controller.WidgetView; } }

        protected TForm GetForm<TForm>() where TForm : WidgetFormViewBase
        {
            return Controller.GetForm<TForm>();
        }

        protected void OnLogoutClicked()
        {
            GetForm<ApproveForm>().ShowChoiceWindow("Are you sure?", DoLogout);  
        }

        protected void OnCloseMarketClicked()
        {
            Controller.Close();
        }

        private void DoLogout(bool result)
        {
            if (result)
            {
                Controller.PreLogout();
            }
        }
    }
}