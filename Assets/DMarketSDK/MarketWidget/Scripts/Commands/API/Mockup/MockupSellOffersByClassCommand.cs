using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using System.Collections.Generic;
using TankGame.DMarketIntegration;

namespace DMarketSDK.Market.Commands.API
{
	public class MockupSellOffersByClassCommand : MockUpLoadItemsCommand
	{
        public MockupSellOffersByClassCommand(ShowingItemsInfo loadParameters) : base(loadParameters)
        {
        }

        protected override List<MarketItemModel> CreateItems()
        {
            var result = base.CreateItems();
            foreach (var item in result)
            {
                item.ClassId = _loadParameters.ClassId;
                item.Tittle = item.ClassId.ToString();
                var codeConverter = new DMarketInfoConverter();
                item.ImageUrl = GetUrlForIcon(codeConverter.GetItemType(item.ClassId));
            }
            return result;
        }
    }
}