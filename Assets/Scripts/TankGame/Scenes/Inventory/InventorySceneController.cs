using System;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Settings;
using DMarketSDK.Domain;
using SHLibrary.StateMachine;
using TankGame.Inventory.DMarketIntegration;
using TankGame.Inventory.Domain;
using TankGame.Inventory.States;
using UnityEngine;
using DMarketSDK.Basic;
using TankGame.Catalogs.Game;
using TankGame.Domain.PlayerData;

namespace TankGame.Inventory
{
    public class InventorySceneController : StateMachineBase<InventoryView>
    {
        //TODO after merge marketWidget and widget need delete parameters
        public class InventoryParameters
        {
            public readonly PlayerInventoryInfo Inventory;
            public readonly ClientApi ClientApi;
            public readonly IWidgetCore Widget;
            public readonly IClientApiSettings ApiSettings;
            public readonly bool IsBasicIntegration;

            public InventoryParameters(PlayerInventoryInfo inventory, ClientApi clientApi, IWidgetCore widget, IClientApiSettings apiSettings, bool isBasicIntegration)
            {
                Inventory = inventory;
                ClientApi = clientApi;
                Widget = widget;
                ApiSettings = apiSettings;
                IsBasicIntegration = isBasicIntegration;
            }
        }

        public Action ToPreviousScene;
        public Action<ItemChangingRequest> MarketItemAction;
        public Action<DMarketLoadDataRequest> LoadDMarketData;
        public Action<DMarketGameTokenRequest> GetGameToken;
        public Action<DMarketLoadDataRequest> LoadGameInventory;
        public Action<DMarketLoadDataRequest> UnloadGameInventory;

        [SerializeField]
        private InventoryImageCatalog _spriteCatalog;
        [SerializeField]
        private GameItemScriptableCatalog _itemInfoCatalog;

        private InventoryParameters _inventoryParameters;

        public InventorySceneModel Model { get; private set; }

        public IWidgetCore Widget
        {
            get { return _inventoryParameters.Widget; }
        }

        public ClientApi ClientApi
        {
            get { return _inventoryParameters.ClientApi; }
        }

        public InventoryIntegrationModel MarketIntegrationModel { get; private set; }

        public bool IsBasicIntegration
        {
            get { return _inventoryParameters.IsBasicIntegration; }
        }

        public void Run(InventoryParameters parameters)
        {
            _inventoryParameters = parameters;

            Model = new InventorySceneModel { IsLoggedDMarket = Widget.IsLogged};
            MarketIntegrationModel = new InventoryIntegrationModel(_spriteCatalog, _itemInfoCatalog);

            ClientApi.ApplyHttpProtocol(parameters.ApiSettings);

            Widget.LoginEvent += WidgetLoginEvent;
            Widget.LogoutEvent += WidgetLogoutEvent;

            UpdateInventoryData(_inventoryParameters.Inventory);

            ApplyState<InventoryInitState>();
        }

        private void WidgetLoginEvent(LoginEventData loginData)
        {
            Model.IsLoggedDMarket = Widget.IsLogged;
            Model.SetChanges();
        }

        private void WidgetLogoutEvent()
        {
            Model.IsLoggedDMarket = Widget.IsLogged;
            Model.SetChanges();
        }

        public void UpdateInventoryData(PlayerInventoryInfo inventory)
        {
            MarketIntegrationModel.Update(inventory);
            Model.SetInventory(inventory);
            Model.SetChanges();
        }

        public void Shutdown()
        {
            Widget.LoginEvent -= WidgetLoginEvent;
            Widget.LogoutEvent -= WidgetLogoutEvent;
        }

        public void CloseInventory()
        {
            ApplyState<InventoryCloseState>();
        }
    }
}