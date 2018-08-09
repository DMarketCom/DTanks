using System;
using System.Collections.Generic;
using DMarketSDK.Market.GameIntegration;
using TankGame.Catalogs.Game;
using TankGame.DMarketIntegration;
using TankGame.Domain.PlayerData;

namespace TankGame.Inventory.DMarketIntegration
{
    public class InventoryIntegrationModel : IGameIntegrationModel
    {
        #region IGameIntegrationModel implementation
        public event Action ItemsChanged;

        public List<InGameItemInfo> Items
        {
            get
            {
                return _items;
            }
        }
        #endregion

        private readonly IInventoryImageCatalog _imageCatalog;
        private readonly IGameItemsInfoCatalog _infoCatalog;
        private readonly List<InGameItemInfo> _items;
        private readonly DMarketInfoConverter _dMarketConverter;

        public InventoryIntegrationModel(IInventoryImageCatalog imageCatalog,
            IGameItemsInfoCatalog infoCatalog)
        {
            _imageCatalog = imageCatalog;
            _infoCatalog = infoCatalog;
            _items = new List<InGameItemInfo>();
            _dMarketConverter = new DMarketInfoConverter();
        }

        public void Update(PlayerInventoryInfo inventory)
        {
            //TODO need optimization
            _items.Clear();

            foreach (PlayerItemInfo gameItem in inventory.Items)
            {
                if (!gameItem.IsInMarket && !inventory.IsEquipped(gameItem.WorldId))
                {
                    var newItem = new InGameItemInfo
                    {
                        Title = _infoCatalog.GetInfo(gameItem.ItemType).Name,
                        Sprite = _imageCatalog.GetInventoryItemSprite(gameItem.ItemType),
                        AssetId = _dMarketConverter.GetAssetId(gameItem.WorldId),
                        ClassId = _dMarketConverter.GetClassId(gameItem.ItemType)
                    };
                    _items.Add(newItem);
                }
            }
            ItemsChanged.SafeRaise();
        }
    }
}