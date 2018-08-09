using DMarketSDK.Market.Commands.API;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Strategy
{
    public class MarketOnlineStrategy : IMarketStrategy
    {
        ApiCommandBase IMarketStrategy.GetChangeItemInventoryCommand(MarketItemModel model, MarketMoveItemType marketMoveType)
        {
            return new ChangeInventoryForItemCommand(model, marketMoveType);
        }

        ApiCommandBase IMarketStrategy.GetCreateSellOfferCommand(MarketItemModel model)
        {
            return new CreateSellOfferCommand(model.AssetId, model.Price.Amount,
                model.Price.Currency);
        }

        ApiCommandBase IMarketStrategy.GetEditSellOfferCommand(MarketItemModel model)
        {
            return new EditSellOfferCommand(model.SellOfferId, model.Price.Amount,
                model.Price.Currency);
        }

        ApiCommandBase IMarketStrategy.GetBuyItemCommand(MarketItemModel model)
        {
            return new BuyItemCommand(model);
        }

        ApiCommandBase IMarketStrategy.GetLoadMySellOffersItemsCommand(ShowingItemsInfo loadInfo)
        {
            return new LoadMySellOffersCommand(loadInfo);
        }

        LoadFiltersCommandBase IMarketStrategy.GetLoadGameCategoriesCommand()
        {
            return new LoadGameCategoriesCommand();
        }

        public ApiCommandBase GetLoadBuyItemsPriceRangeCommand(ShowingItemsInfo itemsInfo)
        {
            return new LoadBuyItemsPriceRangeCommand(itemsInfo);
        }

        ApiCommandBase IMarketStrategy.GetLoadMarketInventoryCommand(ShowingItemsInfo loadInfo)
        {
            return new LoadMarketInventoryCommand(loadInfo);
        }

        LoadFiltersCommandBase IMarketStrategy.GetLoadOrderStatusesCommand()
        {
            return new LoadOrderStatusesCommand();
        }

        LoadFiltersCommandBase IMarketStrategy.GetBuyLoadOrderByCommand()
        {
            return new LoadBuyOfferOrderByListCommand();
        }

        LoadFiltersCommandBase IMarketStrategy.GetSellLoadOrderByCommand()
        {
            return new LoadSellOfferOrderByListCommand();
        }

        ApiCommandBase IMarketStrategy.GetLoadMarketItemsCommand(ShowingItemsInfo loadInfo)
        {
            return new LoadBuyItemsCommand(loadInfo);
        }

        ApiCommandBase IMarketStrategy.GetLoadSellOffersByClassIdCommand(ShowingItemsInfo loadInfo)
        {
            return new LoadSellOffersByClassCommand(loadInfo);
        }

        ApiCommandBase IMarketStrategy.GetCancelSellOfferCommand(MarketItemModel item)
        {
            return new CancelSellOrderCommand(item);
        }

        ApiCommandBase IMarketStrategy.GetUpdateBalanceCommand()
        {
            return new LoadUserBalanceCommand();
        }
    }
}