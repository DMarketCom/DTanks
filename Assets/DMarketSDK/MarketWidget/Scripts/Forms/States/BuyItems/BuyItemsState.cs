using DMarketSDK.Market.Commands.API;
using DMarketSDK.Market.Domain;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.Forms
{
    public class BuyItemsState : MarketItemsStateBase<BuyItemsForm, BuyItemsFormModel>
    {
        protected override MarketTabType MarketTab
        {
            get
            {
                return MarketTabType.BuyItems;
            }
        }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            if (!View.HaveFilters)
            {
                LoadMarketFilters();
            }

            if (!View.HaveOrderByFilters)
            {
                LoadMarketOrderBy();
            }

            LoadFormItems();
        }

        public override void Finish()
        {
            base.Finish();
            View.ClearPriceRange();
        }

        protected override void LoadFormItems()
        {
            base.LoadFormItems();
            LoadPriceRange();
        }

        private void LoadMarketFilters()
        {
            var command = Strategy.GetLoadGameCategoriesCommand();
            command.CommandFinished += OnLoadFilterCommandFinished;
            SendApiCommand(command);
        }

        private void OnLoadFilterCommandFinished(CommandBase command)
        {
            var apiCommand = (LoadFiltersCommandBase)command;
            View.AddFilters(apiCommand.ResultCategories);
            apiCommand.CommandFinished -= OnLoadFilterCommandFinished;
        }

        private void LoadMarketOrderBy()
        {
            var command = Strategy.GetBuyLoadOrderByCommand();
            command.CommandFinished += OnLoadOrderByCommandFinished;
            SendApiCommand(command);
        }

        private void OnLoadOrderByCommandFinished(CommandBase command)
        {
            var apiCommand = (LoadFiltersCommandBase)command;
            View.AddOrderBy(apiCommand.ResultCategories);
            apiCommand.CommandFinished -= OnLoadFilterCommandFinished;
        }

        private void LoadPriceRange()
        {
            var command = Strategy.GetLoadBuyItemsPriceRangeCommand(FormModel.ShowingItemsInfo);
            command.CommandFinished += OnPriceFiltersLoaded;
            SendApiCommand(command);
        }

        private void OnPriceFiltersLoaded(CommandBase command)
        {
            command.CommandFinished -= OnPriceFiltersLoaded;
            var loadPriceRangeCommand = (ILoadPriceRangeCommand) command;
            var commandResult = loadPriceRangeCommand.PriceRangeResult;

            FormModel.ShowingItemsInfo.ChangePriceFilter(commandResult.MinPriceRange, commandResult.MaxPriceRange);

            FormModel.MinPriceRange = commandResult.MinPriceRange;
            FormModel.MaxPriceRange = commandResult.MaxPriceRange;
            FormModel.SetChanges();
        }
    }
}