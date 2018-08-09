using DMarketSDK.IntegrationAPI;
using System.Text.RegularExpressions;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Basic.States
{
    public abstract class BasicWidgetFormStateBase<TFormView, TFormModel> : BasicWidgetStateBase 
        where TFormView : WidgetFormViewBase<TFormModel> 
        where TFormModel : WidgetFormModel, new ()
    {
        protected TFormView View { get; private set; }
        protected TFormModel FormModel { get { return View.FormModel; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            View = Controller.GetForm<TFormView>();
            if (View.Model == null)
            {
                View.ApplyModel(new TFormModel());
            }
            View.Show();
        }

        public override void Finish()
        {
            base.Finish();
            View.Hide();
        }
       
        protected bool IsEmailFormat(string email)
        {
            const string MatchEmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            if (!Regex.IsMatch(email, MatchEmailPattern))
            {
                ShowError(ErrorCode.WrongEmailPattern);
                return false;
            }
            return true;
        }

        protected virtual void ShowError(ErrorCode errorCode)
        {
        }
    }
}