using DMarketSDK.Market.Commands.API;
using DMarketSDK.Market.Domain;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.Forms
{
    public class MySellOffersState : MarketItemsStateBase<MySellOffersForm, ItemsFormModel>
    {
        protected override MarketTabType MarketTab
        {
            get
            {
                return MarketTabType.MySellOffers;
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

        private void LoadMarketOrderBy()
        {
            var command = Strategy.GetSellLoadOrderByCommand();
            command.CommandFinished += OnLoadOrderByCommandFinished;
            SendApiCommand(command);
        }

        private void LoadMarketFilters()
        {
            var command = Strategy.GetLoadOrderStatusesCommand();
            command.CommandFinished += OnLoadFilterCommandFinished;
            SendApiCommand(command);
        }

        private void OnLoadFilterCommandFinished(CommandBase command)
        {
            var apiCommand = (LoadFiltersCommandBase)command;
            View.AddFilters(apiCommand.ResultCategories);
            apiCommand.CommandFinished -= OnLoadFilterCommandFinished;
        }


        private void OnLoadOrderByCommandFinished(CommandBase command)
        {
            var apiCommand = (LoadFiltersCommandBase)command;
            View.AddOrderBy(apiCommand.ResultCategories);
            apiCommand.CommandFinished -= OnLoadFilterCommandFinished;
        }
    }
}