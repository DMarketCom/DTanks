using System;
using System.Collections.Generic;
using DMarketSDK.Domain;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.Market;
using TankGame.Application.Commands;
using TankGame.Catalogs.Scene;
using TankGame.Inventory;
using TankGame.Inventory.Domain;
using TankGame.Network.Client;
using TankGame.Network.Messages;

namespace TankGame.Application.States
{
    public class AppInventorySceneState : AppSceneStateBase<InventorySceneController>
    {
        //TODO refactoring using container for callbacks with id 
        private Action<ItemsChangingResponse> _waitingItemChangingCallback;
        private Action<DMarketDataLoadResponse> _waitingLoadDataCallback;
        private Action<DMarketGameTokenResponse> _waitingGameTokenCallback;
        private Action<DMarketDataLoadResponse> _waitingLoadingGameInventoryCallback;

        //TODO change to mapping with WorldId
        private List<Action<MarketMoveItemResponse>> _waitingMarketCallback = new List<Action<MarketMoveItemResponse>>();

        protected override SceneType SceneName { get { return SceneType.Inventory; } }
        private IAppClient Client { get { return Controller.Client; } }

        protected override void OnSceneStarted()
        {
            SceneController.ToPreviousScene += OnBackClick;
            SceneController.LoadDMarketData += OnLoadDMarketData;
            SceneController.MarketItemAction += OnMarketItemAction;
            SceneController.GetGameToken += OnLoadGameToken;
            SceneController.LoadGameInventory += OnLoadGameInventory;
            SceneController.UnloadGameInventory += OnUnloadGameInventory;

            Controller.Widget.LoginEvent += OnDMarketLogin;
            if (!Controller.IsBasicIntegration)
            {
                (Controller.Widget as IMarketWidget).MoveItemRequest += OnDMarketItemChanged;
            }

            Client.Disconnected += OnDisconnected;
            Client.AppMsgReceived += OnAppMsgReceived;

            //TODO Refactoring after merge marketWidget and widget
            var sceneParameters = new InventorySceneController.InventoryParameters(Model.PlayerModel.Inventory, 
                Controller.WidgetApi, Controller.Widget, Controller.MarketSettings, Controller.IsBasicIntegration);
            SceneController.Run(sceneParameters);
        }

        public override void Finish()
        {
            base.Finish();
            SceneController.ToPreviousScene -= OnBackClick;
            SceneController.LoadDMarketData -= OnLoadDMarketData;
            SceneController.MarketItemAction -= OnMarketItemAction;   
            SceneController.GetGameToken -= OnLoadGameToken;
            SceneController.LoadGameInventory -= OnLoadGameInventory;
            SceneController.UnloadGameInventory -= OnUnloadGameInventory;

            Controller.Widget.LoginEvent -= OnDMarketLogin;
            if (!Controller.IsBasicIntegration)
            {
                (Controller.Widget as IMarketWidget).MoveItemRequest -= OnDMarketItemChanged;
            }

            Client.Disconnected -= OnDisconnected;
            Client.AppMsgReceived -= OnAppMsgReceived;

            SceneController.Shutdown();
        }

        #region SceneController event hanlders

        protected override void OnBackClick()
        {
            base.OnBackClick();
            ApplyPreviousState();
        }

        private void OnLoadDMarketData(DMarketLoadDataRequest request)
        {
            var message = new AppLoadDMarketDataMessage { MarketToken = request.MarketToken };
            _waitingLoadDataCallback = request.Callback;
            Client.Send(message);
        }

        private void OnMarketItemAction(ItemChangingRequest request)
        {
            var message = new AppChangingItemsMessage
            {
                Params =
                {
                    ActionType = request.ActionType,
                    WorldIds = request.WorldIds
                }
            };
            _waitingItemChangingCallback = request.Callback;
            Client.Send(message);
        }

        private void OnLoadGameToken(DMarketGameTokenRequest request)
        {
            var message = new AppGetGameTokenMessage(Model.PlayerModel.AuthInfo.UserName);
            _waitingGameTokenCallback = request.Callback;
            Client.Send(message);
        }

        private void OnUnloadGameInventory(DMarketLoadDataRequest dMarketLoadDataRequest)
        {
            _waitingLoadingGameInventoryCallback = dMarketLoadDataRequest.Callback;
            UnloadPlayerInventory();
        }

        private void OnLoadGameInventory(DMarketLoadDataRequest dMarketLoadDataRequest)
        {
            _waitingLoadingGameInventoryCallback = dMarketLoadDataRequest.Callback;
            LoadPlayerInventory();
        }

        #endregion

        #region Client event handlers

        private void OnAppMsgReceived(AppServerAnswerMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.ItemChangingAnswer:
                    OnItemChangingResponse(message as AppChangingItemsAnswerMessage);
                    break;
                case AppMsgType.LoadDMarketDataAnswer:
                    OnDMarketLoadDataResponse(message as AppLoadDMarketAnswerMessage);
                    break;
                case AppMsgType.UpdatePlayerDataAnswer:
                    OnUpdatePlayerData(message as AppUpdatePlayerDataAnswerMessage);
                    break;
                case AppMsgType.LoadInventoryBasicIntegrationAnswer:
                    OnLoadInventoryBasicIntegration(message as InventoryBasicIntegrationAnswerMessage);
                    break;
                case AppMsgType.GetGameTokenAnswer:
                    OnGetGameToken(message as AppGetGameTokenAnswerMessage);
                    break;
            }
        }

        private void OnDisconnected()
        {
            ApplyState<AppLobbyClientSceneState>("Check connection setting and try again");
        }

        #endregion

        #region NetworkMessages handlers

        private void OnDMarketLoadDataResponse(AppLoadDMarketAnswerMessage message)
        {
            if (_waitingLoadDataCallback != null)
            {
                _waitingLoadDataCallback.SafeRaise(message.Response);
                _waitingLoadDataCallback = null;
            }
        }

        private void OnItemChangingResponse(AppChangingItemsAnswerMessage message)
        {
            if (_waitingItemChangingCallback != null)
            {
                _waitingItemChangingCallback.SafeRaise(message.Response);
                _waitingItemChangingCallback = null;
            }
            else if (_waitingMarketCallback.Count > 0)
            {
                _waitingMarketCallback[0].SafeRaise(
                    new MarketMoveItemResponse(GetErrorText(message.Response), message.Response.MarketError));
                _waitingMarketCallback.RemoveAt(0);
            }
        }

        private void OnGetGameToken(AppGetGameTokenAnswerMessage message)
        {
            if (_waitingGameTokenCallback != null)
            {
                _waitingGameTokenCallback.SafeRaise(message.Response);
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

        private void OnLoadInventoryBasicIntegration(InventoryBasicIntegrationAnswerMessage message)
        {
            _waitingLoadingGameInventoryCallback.SafeRaise(message.Response);
            _waitingLoadingGameInventoryCallback = null;
        }

        #endregion

        #region DMarket event handlers

        private void OnDMarketLogin(LoginEventData loginData)
        {
            Client.Send(new AppUpdateMarketTokenMessage(loginData.MarketAccessToken, loginData.MarketRefreshAccessToken));
        }

        private void OnDMarketItemChanged(MarketMoveItemRequestParams transactionData)
        {
            var message = new AppDMarketTransactionMessage(transactionData);
            _waitingMarketCallback.Add(transactionData.Callback);
            Client.Send(message);
        }

        #endregion
        
        private string GetErrorText(ItemsChangingResponse messageResponse)
        {
            if (messageResponse.IsSuccess)
            {
                return string.Empty;
            }

            if (messageResponse.MarketError != ErrorCode.None)
            {
                return Controller.Widget.GetErrorMessage(messageResponse.MarketError);
            }

            return messageResponse.ErrorText;
        }

        private void LoadPlayerInventory()
        {
            Client.Send(new InventoryBasicIntegrationMessage());
        }

        private void UnloadPlayerInventory()
        {
            Client.Send(new UnloadInventoryBasicIntegrationMessage());
        }
    }
}