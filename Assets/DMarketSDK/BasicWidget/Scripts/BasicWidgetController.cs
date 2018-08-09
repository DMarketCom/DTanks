using DMarketSDK.Common.ErrorHelper;
using DMarketSDK.IntegrationAPI;
using SHLibrary.Logging;
using System;
using DMarketSDK.Basic.States;
using DMarketSDK.IntegrationAPI.Request.Auth;
using SHLibrary.StateMachine;
using UnityEngine;
using DMarketSDK.Common.UI;
using DMarketSDK.Domain;
using DMarketSDK.Forms;
using DMarketSDK.FormsCatalog;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Basic
{
    public sealed class BasicWidgetController : StateMachineBase, IBasicWidget
    {
        #region IBasicWidget implementation

        public event Action<LoginEventData> LoginEvent;
        public event Action PreLogoutEvent;
        public event Action LogoutEvent;
        public event Action<Error> ErrorEvent;
        public event Action ClosedEvent;

        public bool IsLogged { get { return Model != null && !string.IsNullOrEmpty(Model.UserName); } }
        public bool IsInitialized { get { return Model != null; } }

        public void Init(string basicAccessToken, string basicRefreshToken, ClientApi clientApi)
        {
            Model = new WidgetModel();
            Model.SetBasicTokens(basicAccessToken, basicRefreshToken);
            MarketApi = clientApi;
            MobileInputFieldHandler.InputSelected += OnMobileInputFieldSelected;
        }

        public void Open()
        {
            if (!IsInitialized)
            {
                DevLogger.Error("Initialize MarketWidget before use.");
                return;
            }

            _formContainer.Run();

            WidgetView = GetForm<BasicView>();
            WidgetView.ApplyModel(Model);

            ApplyState<BasicWidgetOpeningState>();
        }

        public void Close()
        {
            ApplyState<BasicWidgetClosedState>();
            _formContainer.Stop();
            ClosedEvent.SafeRaise();
        }

        public void PreLogout()
        {
            var widgetWaitingForm = GetForm<WidgetWaitingForm>();
            widgetWaitingForm.Show();
            PreLogoutEvent.SafeRaise();
        }

        /// <summary>
        /// In BasicWidget you need implicit call Logout after PreLogout, for possibility make
        /// a bulk transfer of player game inventory to DMarket inventory in one DMarket session.
        /// </summary>
        public void Logout()
        {
            var widgetWaitingForm = GetForm<WidgetWaitingForm>();
            widgetWaitingForm.Hide();

            Model.ClearUserData();
            MarketApi.Logout(Model.BasicAccessToken, Model.MarketAccessToken, OnLogoutCallback, OnErrorCallback);
        }

        public void Dispose()
        {
            Model = null;
            _formContainer.Stop();
            MobileInputFieldHandler.InputSelected -= OnMobileInputFieldSelected;
        }

        public string GetErrorMessage(ErrorCode errorCode)
        {
            return ErrorHelper.GetErrorMessage(errorCode);
        }

        #endregion

        [SerializeField]
        private WidgetFormCreator _formContainer;
        [SerializeField]
        private ApiErrorHelper _apiErrorHepler;

        public WidgetModel Model { get; private set; }

        public BasicView WidgetView { get; private set; }

        public ClientApi MarketApi { get; private set; }

        public IApiErrorHelper ErrorHelper { get { return _apiErrorHepler; } }

        protected override void OnStateStarted()
        {
            base.OnStateStarted();
#if UNITY_EDITOR
            gameObject.name = State.ToString();
#endif
        }

        public void Login(string login, string marketAccessToken, string marketRefreshToken)
        {
            Model.UserName = login;
            Model.SetMarketTokens(marketAccessToken, marketRefreshToken);
            Model.SetChanges();

            MarketApi.MarketToken = marketAccessToken;

            LoginEvent.SafeRaise(new LoginEventData(marketAccessToken, marketRefreshToken, login));
        }

        public T GetForm<T>() where T : WidgetFormViewBase
        {
            return _formContainer.GetForm<T>();
        }

        public void ApplyScreenSettings(ScreenOrientationSettings settings)
        {
            Screen.autorotateToLandscapeLeft = settings.RotateToLandscapeLeft;
            Screen.autorotateToLandscapeRight = settings.RotateToLandscapeRight;
            Screen.autorotateToPortrait = settings.RotateToPortraitUp;
            Screen.autorotateToPortraitUpsideDown = settings.RotateToPortraitDown;

            Model.SetScreenOrientationSettings(settings);
            Model.SetChanges();
        }

        private void OnLogoutCallback(LogoutRequest.Response result, LogoutRequest.RequestParams request)
        {
            LogoutEvent.SafeRaise();

            ApplyState<BasicWidgetLoginFormState>();
        }

        private void OnErrorCallback(Error error)
        {
            ErrorEvent.SafeRaise(error);
        }

        private InputFieldForm OnMobileInputFieldSelected()
        {
            return GetForm<InputFieldForm>();
        }
    }
}