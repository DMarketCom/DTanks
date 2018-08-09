using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using System.Collections.Generic;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadMarketInventoryCommand : ApiCommandBase, ILoadMarketItemsCommand
    {
        private readonly ShowingItemsInfo _loadParameters;

        public LoadMarketItemsCommandResult CommandResult { get; private set; }

        public LoadMarketInventoryCommand(ShowingItemsInfo loadParameters)
        {
            _loadParameters = loadParameters;
        }

        public override void Start()
        {
            base.Start();
            MarketApi.LoadMarketInventoryRequest(_loadParameters.Limit, _loadParameters.Offset, 
                _loadParameters.SearchPattern, OnSuccessCallback, OnError);
        }

        private void OnSuccessCallback(GetUserInventoryRequest.Response response, GetUserInventoryRequest.RequestParams request)
        {
            var items = new List<MarketItemModel>();
            for (int i = 0; i < response.Items.Count; i++)
            {
                var itemInfo = response.Items[i];
                var itemModel = new MarketItemModel
                {
                    Tittle = itemInfo.title,
                    ClassId = itemInfo.classId,
                    AssetId = itemInfo.assetId,
                    ImageUrl = itemInfo.imageUrl
                };
                items.Add(itemModel);
            }

            CommandResult = new LoadMarketItemsCommandResult(items, response.total);
            Terminate(true);
        }
    }
}