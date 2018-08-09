using DMarketSDK.Common.ErrorHelper;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.Market.GameIntegration;
using DMarketSDK.Market.PopUps;
using DMarketSDK.Market.States;
using DMarketSDK.Market.Strategy;
using SHLibrary.Logging;
using System;
using DMarketSDK.Common.Sound;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.Market.Forms;
using SHLibrary.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using DMarketSDK.Common.Forms;
using DMarketSDK.Common.UI;
using DMarketSDK.Domain;
using DMarketSDK.Forms;
using DMarketSDK.FormsCatalog;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Market
{
    public sealed class MarketWidgetController : StateMachineBase, IMarketWidget
    {
        #region IMarketWidget implementation

        public event Action<LoginEventData> LoginEvent;
        public event Action LogoutEvent;
        public event Action<Error> ErrorEvent;
        public event Action ClosedEvent;
        public event Action<MarketMoveItemRequestParams> MoveItemRequest;

        public bool IsLogged
        {
            get { return Model != null && !string.IsNullOrEmpty(Model.UserName); }
        }

        public bool IsInitialized
        {
            get { return Model != null; }
        }

        public float Volume
        {
            get { return _soundManager.Volume; }
            set { _soundManager.Volume = value; }
        }

        public void Init(string basicAccessToken, string basicRefreshToken, ClientApi clientApi)
        {
            Model = new WidgetModel();
            Model.SetBasicTokens(basicAccessToken, basicRefreshToken);
            MarketApi = clientApi;
            MobileInputFieldHandler.InputSelected += OnMobileInputFieldSelected;
        }

        public void Open(IGameIntegrationModel gameItems)
        {
            if (!IsInitialized)
            {
                DevLogger.Error("Initialize MarketWidget before use.");
                return;
            }

            ApplyMarketStrategy();
            GameModel = gameItems;
            _formContainer.Run();
            var textPopUp = GetForm<MarketTextPopUp>();
            PopUpController.Initialize(textPopUp);
            _soundManager.IsRuning = true;

            View = GetForm<MarketView>();
            View.ApplyModel(Model);

            ApplyState<MarketOpeningState>();
        }

        public void Close()
        {
            ApplyState<MarketClosedState>();
            _soundManager.IsRuning = false;
            _formContainer.Stop();
            ClosedEvent.SafeRaise();
        }

        public void Logout()
        {
            Model.ClearUserData();
            MarketApi.Logout(Model.BasicAccessToken, Model.MarketAccessToken, OnLogoutCallback, OnErrorCallback);
        }

        public void Dispose()
        {
            Model = null;
            _soundManager.IsRuning = false;
            _formContainer.Stop();
            MobileInputFieldHandler.InputSelected -= OnMobileInputFieldSelected;
        }

        public string GetErrorMessage(ErrorCode errorCode)
        {
            return ErrorHelper.GetErrorMessage(errorCode);
        }

#endregion

        [SerializeField] private WidgetFormCreator _formContainer;
        [SerializeField] private ApiErrorHelper _apiErrorHepler;
        [SerializeField] private MarketPopUpContainer _marketPopUpController;
        [SerializeField] private MarketSoundManager _soundManager;

        public WidgetModel Model { get; private set; }

        public MarketView View { get; private set; }

        public ClientApi MarketApi { get; private set; }

        public IGameIntegrationModel GameModel { get; private set; }

        public IMarketStrategy Strategy { get; private set; }

        public IApiErrorHelper ErrorHelper
        {
            get { return _apiErrorHepler; }
        }

        public MarketPopUpContainer PopUpController
        {
            get { return _marketPopUpController; }
        }

        public IMarketSoundManager SoundManager
        {
            get { return _soundManager; }
        }

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

        public void MoveItemAcrossInventory(MarketMoveItemRequestParams request)
        {
            if (MoveItemRequest != null)
            {
                MoveItemRequest.Invoke(request);
            }
            else
            {
                request.Callback.SafeRaise(new MarketMoveItemResponse(
                    "Not implement in game side. Subscribe to IMarketWidget change event"));
            }
        }

        private void OnLogoutCallback(LogoutRequest.Response result, LogoutRequest.RequestParams request)
        {
            LogoutEvent.SafeRaise();

            ApplyState<MarketLoginFormState>();
        }

        private void OnErrorCallback(Error error)
        {
            ErrorEvent.SafeRaise(error);
        }

        private void ApplyMarketStrategy()
        {
            if (SceneManager.GetActiveScene().name.Equals("TestMarket"))
            {
                DevLogger.Log("Apply mockup models");
                Strategy = new MarketMockupStrategy();
            }
            else
            {
                Strategy = new MarketOnlineStrategy();
            }
        }

        private InputFieldForm OnMobileInputFieldSelected()
        {
            return GetForm<InputFieldForm>();
        }
    }
}