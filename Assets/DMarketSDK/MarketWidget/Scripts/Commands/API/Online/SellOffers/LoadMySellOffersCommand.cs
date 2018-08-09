using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Request.MarketIntegration;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadMySellOffersCommand : ApiCommandBase, ILoadMarketItemsCommand
    {
        private readonly ShowingItemsInfo _loadParameters;

        public LoadMarketItemsCommandResult CommandResult { get; private set; }

        public LoadMySellOffersCommand(ShowingItemsInfo loadParameters)
        {
            _loadParameters = loadParameters;
        }

        public override void Start()
        {
            base.Start();
            MarketApi.LoadSellOffersRequest(_loadParameters.Limit, _loadParameters.Offset,
                _loadParameters.SearchPattern, _loadParameters.OrderBy,
                _loadParameters.GetDirByBody(), _loadParameters.GetCategoriesBody(),
                OnSuccessCallback, OnError);
        }

        private void OnSuccessCallback(GetUserSellOffersRequest.Response response, GetUserSellOffersRequest.RequestParams request)
        {
            var answer = response;
            var modelsList = new List<MarketItemModel>();

            foreach (var itemInfo in answer.Items)
            {
                var itemModel = new MarketItemModel
                {
                    SellOfferId = itemInfo.sellOfferId,
                    Tittle = itemInfo.title,
                    ImageUrl = itemInfo.imageUrl,
                    Created = IntToDateTime(itemInfo.created),
                    Fee = itemInfo.fee,
                    Price = itemInfo.price,
                    Status = SellOfferStatusTypeExtentions.GetSellOfferStatusType(itemInfo.status)
                };
                itemModel.Fee.Currency = itemModel.Price.Currency; // TODO: workaround. wait backend.

                modelsList.Add(itemModel);
            }

            CommandResult = new LoadMarketItemsCommandResult(modelsList, answer.total);
            Terminate(true);
        }
    }
}
