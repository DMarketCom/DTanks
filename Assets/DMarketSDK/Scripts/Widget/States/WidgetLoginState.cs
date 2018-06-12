using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public class WidgetLoginState : WidgetAutorizationStateBase<WidgetLoginForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.SignUpButton.onClick.AddListener(OnSignUpClicked);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.SignUpButton.onClick.RemoveListener(OnSignUpClicked);
        }

        protected override void ApplySendingCommand()
        {
            Controller.ClientApi.GetMarketAccessToken(Model.BasicAccessToken,
                Login, Password,
                OnGetTokenCallback,
                OnErrorCallback);
        }

        private void OnGetTokenCallback(TokenRequest.Response data, TokenRequest.RequestParams request)
        {
            FormView.SetWaitState(false);

            if (string.IsNullOrEmpty(data.token))
            {
                return;
            }
            Model.MarketAccessToken = data.token;
            Model.MarketRefreshAccessToken = data.refreshToken;
            Model.LoggedUsername = Login;
            Model.SetChanges();

            ApplyState<WidgetLoggedState>();
        }

        protected override void OnErrorCallback(Error error)
        {
            if (error.ErrorCode == ErrorCode.BasicAccessTokenExpired)
            {
                Controller.ClientApi.GetBasicRefreshToken(Model.BasicRefreshToken, 
                    GetBasicRefreshTokenCallback, OnErrorCallback);
            }
            else
            {
                base.OnErrorCallback(error);
            }
        }

        private void GetBasicRefreshTokenCallback(BasicRefreshTokenRequest.Response result,
            BasicRefreshTokenRequest.RequestParams request)
        {
            //TODO sending refresh to server if server have token cash
            Model.ApplyTokens(result.token, result.refreshToken);
            Model.SetChanges();
            ApplySendingCommand();
        }

        private void OnSignUpClicked()
        {
            ApplyState<WidgetRegistrationState>();
        }
    }
}