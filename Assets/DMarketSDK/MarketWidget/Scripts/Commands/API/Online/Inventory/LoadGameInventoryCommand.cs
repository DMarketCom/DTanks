using System.Collections.Generic;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using DMarketSDK.Market.GameIntegration;
using UnityEngine;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadGameInventoryCommand : MarketCommandBase, ILoadMarketItemsCommand
    {
        private readonly ShowingItemsInfo _loadParameters;

        private IGameIntegrationModel GameModel { get { return Controller.GameModel; } }

        public LoadMarketItemsCommandResult CommandResult { get; private set; }

        public LoadGameInventoryCommand(ShowingItemsInfo loadParameters)
        {
            _loadParameters = loadParameters;
        }

        public override void Start()
        {
            base.Start();
            var targetGameItems = GetTargetGameItems();

            var startIndex = _loadParameters.Offset;
            var endIndex = Mathf.Min(_loadParameters.Offset + _loadParameters.Limit, GameModel.Items.Count);

            var items = new List<MarketItemModel>();
            for (int i = startIndex; i < endIndex  && i < targetGameItems.Count; i++)
            {
                var itemInfo = targetGameItems[i];

                var itemModel = new MarketItemModel
                {
                    Tittle = itemInfo.Title,
                    ClassId = itemInfo.ClassId,
                    AssetId = itemInfo.AssetId,
                    IconSprite = itemInfo.Sprite
                };

                items.Add(itemModel);
            }

            CommandResult = new LoadMarketItemsCommandResult(items, targetGameItems.Count);
            Terminate();
        }

        private List<InGameItemInfo> GetTargetGameItems()
        {
            var result = new List<InGameItemInfo>();
            foreach (var item in GameModel.Items)
            {
                var checkByCategories = !_loadParameters.HaveCategoriesFilter ||
                    _loadParameters.Categories.Contains(item.Category);
                var checkBySearch = !_loadParameters.HaveSearchFilter ||
                    item.Title.ToLower().Contains(_loadParameters.SearchPattern.ToLower());
                if (checkByCategories && checkBySearch)
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}