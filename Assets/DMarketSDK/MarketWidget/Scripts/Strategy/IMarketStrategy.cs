using DMarketSDK.Market.Commands.API;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Strategy
{
    public interface IMarketStrategy
    {
        ApiCommandBase GetLoadMarketInventoryCommand(ShowingItemsInfo loadInfo);

        ApiCommandBase GetLoadMarketItemsCommand(ShowingItemsInfo loadInfo);

        ApiCommandBase GetLoadMySellOffersItemsCommand(ShowingItemsInfo loadInfo);

        ApiCommandBase GetLoadSellOffersByClassIdCommand(ShowingItemsInfo loadInfo);
        
        ApiCommandBase GetCancelSellOfferCommand(MarketItemModel item);

        ApiCommandBase GetCreateSellOfferCommand(MarketItemModel item);

        ApiCommandBase GetEditSellOfferCommand(MarketItemModel item);

        ApiCommandBase GetBuyItemCommand(MarketItemModel item);

        ApiCommandBase GetChangeItemInventoryCommand(MarketItemModel model, MarketMoveItemType marketMoveType);

        ApiCommandBase GetUpdateBalanceCommand();
        
        LoadFiltersCommandBase GetLoadOrderStatusesCommand();

        LoadFiltersCommandBase GetSellLoadOrderByCommand();

        LoadFiltersCommandBase GetBuyLoadOrderByCommand();

        LoadFiltersCommandBase GetLoadGameCategoriesCommand();

        ApiCommandBase GetLoadBuyItemsPriceRangeCommand(ShowingItemsInfo itemsInfo);
    }
}