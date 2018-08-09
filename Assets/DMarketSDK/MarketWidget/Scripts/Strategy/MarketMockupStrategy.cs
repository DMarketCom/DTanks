using DMarketSDK.Market.Commands.API;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Strategy
{
    public class MarketMockupStrategy : IMarketStrategy
    {
        #region IMarketStrategy implemenation

        ApiCommandBase IMarketStrategy.GetChangeItemInventoryCommand(MarketItemModel model, MarketMoveItemType marketMoveType)
        {
            return new MockupItemOperationCommand(model);
        }

        ApiCommandBase IMarketStrategy.GetCreateSellOfferCommand(MarketItemModel model)
        {
            return GetDefaultErrorCommand();
        }

        ApiCommandBase IMarketStrategy.GetEditSellOfferCommand(MarketItemModel model)
        {
            return GetDefaultErrorCommand();
        }

        ApiCommandBase IMarketStrategy.GetBuyItemCommand(MarketItemModel model)
        {
            return GetDefaultErrorCommand();
        }

        ApiCommandBase IMarketStrategy.GetLoadMySellOffersItemsCommand(ShowingItemsInfo loadInfo)
        {
            return new MockUpLoadItemsCommand(loadInfo);
        }

        LoadFiltersCommandBase IMarketStrategy.GetLoadGameCategoriesCommand()
        {
            return new MockupLoadGameCategoriesCommand();
        }

        public ApiCommandBase GetLoadBuyItemsPriceRangeCommand(ShowingItemsInfo itemsInfo)
        {
            return new MockupLoadBuyItemsPriceRangeCommand(itemsInfo);
        }

        ApiCommandBase IMarketStrategy.GetLoadMarketInventoryCommand(ShowingItemsInfo loadInfo)
        {
            return new MockUpLoadItemsCommand(loadInfo);
        }

        LoadFiltersCommandBase IMarketStrategy.GetLoadOrderStatusesCommand()
        {
            return new MockupLoadOrderStatusesCommand();
        }

        LoadFiltersCommandBase IMarketStrategy.GetSellLoadOrderByCommand()
        {
            return new MockupLoadOrderByListCommand();
        }

        LoadFiltersCommandBase IMarketStrategy.GetBuyLoadOrderByCommand()
        {
            return new MockupLoadOrderByListCommand();
        }

        ApiCommandBase IMarketStrategy.GetLoadMarketItemsCommand(ShowingItemsInfo loadInfo)
        {
            return new MockUpLoadItemsCommand(loadInfo);
        }

        ApiCommandBase IMarketStrategy.GetLoadSellOffersByClassIdCommand(ShowingItemsInfo loadInfo)
        {
            return new MockupSellOffersByClassCommand(loadInfo);
        }

        ApiCommandBase IMarketStrategy.GetCancelSellOfferCommand(MarketItemModel item)
        {
            return new MockupCancelSellOfferCommand(item.SellOfferId);
        }

        ApiCommandBase IMarketStrategy.GetUpdateBalanceCommand()
        {
            return new MockupLoadUserBalance();
        }

        #endregion

        private ApiCommandBase GetDefaultErrorCommand()
        {
            return new MockupErrorCommand();
        }
    }
}