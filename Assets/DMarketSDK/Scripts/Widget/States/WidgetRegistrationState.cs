using System.Text.RegularExpressions;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public class WidgetRegistrationState : WidgetAutorizationStateBase<WidgetRegisterForm>
    {   
        protected string Email { get { return FormView.EmailField.text; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.BtnLogin.onClick.AddListener(OnLoginClick);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.BtnLogin.onClick.RemoveListener(OnLoginClick);
        }

        protected override void ApplySendingCommand()
        {
            Controller.ClientApi.RegisterMarketAccount(Model.BasicAccessToken,
                Email, Password, Login,
                OnCallback,
                OnErrorCallback);
        }

        protected override bool CheckCorrectInputFormat()
        {
            return base.CheckCorrectInputFormat() && EmailFormatCheck();
        }

        private bool EmailFormatCheck()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ShowError(WidgetErrorType.EmptyEmail);
                return false;
            }

            if (!IsEmailFormat(Email))
            {
                ShowError(WidgetErrorType.WrongEmailPattern);
                return false;
            }
            return true;
        }

        private void OnCallback(RegisterRequest.Response data, RegisterRequest.RequestParams request)
        {
            FormView.SetWaitState(false);
            ApplyState<WidgetVerifyAccountState>();
        }

        private void OnLoginClick()
        {
            ApplyState<WidgetLoginState>();
        }

        private bool IsEmailFormat(string email)
        {
            const string MatchEmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.IsMatch(email, MatchEmailPattern);
        }
    }
}