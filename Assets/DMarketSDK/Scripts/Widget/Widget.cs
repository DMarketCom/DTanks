using System;
using DMarketSDK.IntegrationAPI;
using SHLibrary.StateMachine;
using DMarketSDK.Widget.States;
using SHLibrary.Utils;
using SHLibrary.Logging;
using UnityEngine;
using DMarketSDK.Common.ErrorHelper;
using DMarketSDK.IntegrationAPI.Request.Auth;

namespace DMarketSDK.Widget
{
    public class Widget : StateMachineBase<WidgetView>, IWidget
    {
        [SerializeField]
        private ApiErrorHelper _apiErrorHepler;

        [SerializeField]
        private WidgetErrorHelper _widgetErrorHelper;

        public IWidgetErrorHelper WidgetErrorHelper { get { return _widgetErrorHelper; } }

        public IApiErrorHelper ApiErrorHelper { get { return _apiErrorHepler; } }

        public ClientApi ClientApi { private set; get; }

        public WidgetModel Model { private set; get; }

        #region IWidget implementation

        public event Action<LoginEventData> LoginEvent;

        public event Action LogoutEvent;

        public event Action ClosedEvent;

        public event Action<Error> ErrorEvent;

        string IWidget.MarketAccessToken { get { return Model.MarketAccessToken; } }

        string IWidget.LoggedUserName
        { get { return Model.LoggedUsername; } }

        bool IWidget.IsInitialize { get { return Model != null; } }

        bool IWidget.IsLogged { get { return Model != null && Model.IsLogin; } }

        void IWidget.Init(ClientApi clientApi, string gameToken, string refreshToken)
        {
            ClientApi = clientApi;
            Model = new WidgetModel();
            Model.ApplyTokens(gameToken, refreshToken);
            View.ApplyModel(Model);
        }

        void IWidget.Open()
        {
            if ((this as IWidget).IsInitialize)
            {
                ApplyState<WidgetOpeningState>();
            }
            else
            {
                DevLogger.Error("Need initialize widget before opening");
            }
        }

        void IWidget.Close()
        {
            ClosedEvent.SafeRaise();
            ApplyState<WidgetClosedState>();
        }

        void IWidget.Logout()
        {
            if (Model.IsLogin)
            {
                ClientApi.Logout(Model.BasicAccessToken, Model.MarketAccessToken, 
                    OnLogoutCallback, OnErrorCallback);
            }
            else
            {
                DevLogger.Warning("Try logout when user is not login");
            }
        }

        void IWidget.SetLoggedUserData(WidgetUserDataModel userDataModel)
        {
            Model.SetLoggedUserData(userDataModel);

            if (Model.IsLogin)
            {
                ApplyState<WidgetLoggedState>();
            }
        }
        #endregion

        private void OnLogoutCallback(LogoutRequest.Response data, LogoutRequest.RequestParams request)
        {
            ApplyState<WidgetLogoutState>();
        }

        public void OnErrorCallback(Error error)
        {
            ErrorEvent.SafeRaise(error);
        }

        protected override void OnStateFinished()
        {
            base.OnStateFinished();
            if (State is WidgetLoginState)
            {
                if (Model.IsLogin)
                {
                    LoginEvent.SafeRaise(new LoginEventData(Model.MarketAccessToken, 
                        Model.MarketRefreshAccessToken, Model.LoggedUsername));
                }
            }
            if (State is WidgetLogoutState)
            {
                if (!Model.IsLogin)
                {
                    LogoutEvent.SafeRaise();
                }
            }
        }
    }
}