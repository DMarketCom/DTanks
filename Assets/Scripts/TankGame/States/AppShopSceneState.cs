using Commands.Client;
using DMarketSDK.Widget;
using Networking.Client;
using Networking.Msg;
using ScenesContainer;
using SHLibrary.Utils;
using Shop.Domain;
using System;
using TankGame.Shop;

namespace States
{
    public class AppShopSceneState : AppSceneStateBase<ShopSceneController>
    {
        //TODO refactoring using container for callbacks with id 
        private Action<ItemsChangingResponse> _waitingItemChangingCallback;
        private Action<DMarketDataLoadResponce> _waitingLoadDataCallback;
        private Action<DMarketGameTokenResponce> _waitingGameTokenCallback;

        private AppDetectInternetCommand _internetDetection;

        protected override SceneType SceneName
        {
            get
            {
                return SceneType.Shop;
            }
        }

        public IAppClient Client { get { return Controller.Client; } }

        protected override void OnSceneStarted()
        {
            SceneController.BackClicked += OnBackClick;
            SceneController.LoadDMarketData += OnLoadDMarketData;
            SceneController.MakedItemAction += OnMakedItemAction;
            SceneController.GetGameToken += OnLoadGameToken;
            Controller.Widget.LoginEvent += OnDMarketLogin;
            Client.Disconected += OnDisconected;
            Client.AppMsgReceived += OnAppMsgReceived;

            _internetDetection = new AppDetectInternetCommand();
            _internetDetection.Disconected += OnDisconected;
            ApplyCommand(_internetDetection);

            SceneController.Run(Model.PlayerModel.Inventory, Controller.Widget, 
                Controller.WidgetApi);
        }
        
        public override void Finish()
        {
            base.Finish();
            SceneController.BackClicked -= OnBackClick;
            SceneController.LoadDMarketData -= OnLoadDMarketData;
            SceneController.MakedItemAction -= OnMakedItemAction;
            SceneController.Shutdown();
            SceneController.GetGameToken -= OnLoadGameToken;
            Controller.Widget.LoginEvent -= OnDMarketLogin;
            Client.Disconected -= OnDisconected;
            Client.AppMsgReceived -= OnAppMsgReceived;
            _internetDetection.Disconected -= OnDisconected;
            SceneController.Shutdown();
        }

        private void OnDisconected()
        {
            ApplyState<AppLobbyClientSceneState>("Check connection setting and try again");
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            ApplyPreviousState();
        }

        private void OnLoadDMarketData(DMarketLoadDataRequest request)
        {
            var message = new AppLoadDMarketDataMessage();
            message.MarketToken = request.MarketToken;
            _waitingLoadDataCallback = request.Callback;
            Client.Send(message);
        }

        private void OnMakedItemAction(ItemChangingRequest request)
        {
            var message = new AppChangingItemsMessage();
            message.ActionType = request.ActionType;
            message.WorldId = request.WorldId;
            _waitingItemChangingCallback = request.Callback;
            Client.Send(message);
        }

        private void OnLoadGameToken(DMarketGameTokenRequest request)
        {
            var message = new AppGetGameTokenMessage(Model.PlayerModel.Autorziation.UserName);
            _waitingGameTokenCallback = request.Callback;
            Client.Send(message);
        }

        private void OnAppMsgReceived(AppServerAnswerMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.ItemChangingAnswer:
                    OnItemChangingResponce(message as AppChangingItemsAnswerMessage);
                    break;
                case AppMsgType.LoadDMarketDataAnswer:
                    OnDMarketLoadDataResponce(message as AppLoadDMarketAnswerMessage);
                    break;
                case AppMsgType.UpdatePlayerDataAnswer:
                    OnUpdatePlayerData(message as AppUpdatePlayerDataAnswerMessage);
                    break;
                case AppMsgType.GetGameTokenAnswer:
                    OnGetGameToken(message as AppGetGameTokenAnswerMessage);
                    break;
            }
        }

        private void OnDMarketLoadDataResponce(AppLoadDMarketAnswerMessage message)
        {
            if (_waitingLoadDataCallback != null)
            {
                _waitingLoadDataCallback.SafeRaise(message.Responce);
                _waitingLoadDataCallback = null;
            }
        }

        private void OnItemChangingResponce(AppChangingItemsAnswerMessage message)
        {
            if (_waitingItemChangingCallback != null)
            {
                _waitingItemChangingCallback.SafeRaise(message.Response);
                _waitingItemChangingCallback = null;
            }
        }

        private void OnGetGameToken(AppGetGameTokenAnswerMessage message)
        {
            if (_waitingGameTokenCallback != null)
            {
                _waitingGameTokenCallback.SafeRaise(message.Responce);
                _waitingGameTokenCallback = null;
            }
        }

        private void OnUpdatePlayerData(AppUpdatePlayerDataAnswerMessage message)
        {
            if (!message.HasError)
            {
                SceneController.UpdateInventoryData(message.Data.Inventory);
            }
        }

        private void OnDMarketLogin(LoginEventData login)
        {
            var message = new AppUpdateMarketTokenMessage
            {
                MarketToken = login.MarketAccessToken,
                RefreshMarketToken = login.MarketRefreshAccessToken
            };
            Client.Send(message);
        }
    }
}