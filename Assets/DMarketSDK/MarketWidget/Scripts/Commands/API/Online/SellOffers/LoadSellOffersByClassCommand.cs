using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using System.Collections.Generic;

namespace DMarketSDK.Market.Commands.API
{
	public class LoadSellOffersByClassCommand : ApiCommandBase, ILoadMarketItemsCommand
	{
		private readonly ShowingItemsInfo _loadParameters;

	    public LoadMarketItemsCommandResult CommandResult { get; private set; }

        public LoadSellOffersByClassCommand(ShowingItemsInfo loadParameters)
		{
			_loadParameters = loadParameters;
		}

		public override void Start()
		{
			base.Start();
			MarketApi.GetAggregatedSellOffersByClassId(_loadParameters.ClassId, _loadParameters.Limit, _loadParameters.Offset,
				OnSuccessCallback, OnError);
		}

		private void OnSuccessCallback(GetGameClassSellOffersRequest.Response response, GetGameClassSellOffersRequest.RequestParams request)
		{
			var items = new List<MarketItemModel>();

			foreach (var itemInfo in response.Items)
			{
			    var itemModel = new MarketItemModel
			    {
			        ClassId = _loadParameters.ClassId,
			        SellOfferId = itemInfo.sellOfferId,
			        Tittle = itemInfo.title,
			        ImageUrl = itemInfo.imageUrl,
			        Created = IntToDateTime(itemInfo.created),
			        Fee = itemInfo.fee,
			        Price = itemInfo.price
			    };
			    itemModel.Fee.Currency = itemModel.Price.Currency;   // TODO: workaround. wait backend.
			    items.Add(itemModel);
			}

		    CommandResult = new LoadMarketItemsCommandResult(items, response.total);
			Terminate(true);
		}
	}
}


