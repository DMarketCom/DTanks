using System;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.Widget;
using PlayerData;
using Shop;
using Shop.Domain;
using Shop.SpriteCatalog;
using Shop.States;
using SHLibrary.StateMachine;
using UnityEngine;

namespace TankGame.Shop
{
    public class ShopSceneController : StateMachineBase<ShopView>
    {
        public Action BackClicked;
        public Action<ItemChangingRequest> MakedItemAction;
        public Action<DMarketLoadDataRequest> LoadDMarketData;
        public Action<DMarketGameTokenRequest> GetGameToken;

        [SerializeField]
        private ShopImageCatalog _spriteCatalog;
        [SerializeField]
        private GameItemScriptableCatalog _itemInfoCatalog;

        public ShopModel Model { get; private set; }
        
        public IWidget Widget { get; private set; }

        public ClientApi WidgetApi { get; private set; }

        public void Run(PlayerInventoryInfo inventory, IWidget widget = null,
            ClientApi widgetApi = null)
        {
            Model = new ShopModel();
            Widget = widget;
            WidgetApi = widgetApi;
            UpdateInventoryData(inventory);
            ApplyState<ShopInitState>();
        }

        public void UpdateInventoryData(PlayerInventoryInfo inventory)
        {
            if (inventory != null) {
                Model.Inventory = inventory;
                Model.SetChanges();
            }
        }

        public void Shutdown()
        {
            ApplyState<ShopCloseState>();
        }
    }
}