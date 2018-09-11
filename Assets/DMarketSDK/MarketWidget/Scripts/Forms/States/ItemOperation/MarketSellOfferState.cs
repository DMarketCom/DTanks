using DMarketSDK.Market.Items;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.Forms
{
    public class MarketSellOfferState : MarketFormStateBase<SellOfferForm, ItemInfoFormModel>
    {
        protected override bool IsNeedChangeTabOnStart
        {
            get { return false; }
        }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            FormModel.CurrentItem = (MarketItemModel)args[0];
            FormModel.Type = (ItemActionType)args[1];
            FormModel.SetChanges();

            View.Closed += OnFormClosed;
            View.PriceEntered += OnPriceEntered;

            if (FormModel.Type == ItemActionType.CreateSellOffer && WidgetModel.CurrentItemType == ItemModelType.GameInventory)
            {
                ShowCannotCreateNotification();
            }
        }

        public override void Finish()
        {
            base.Finish();
            View.Closed -= OnFormClosed;
            View.PriceEntered -= OnPriceEntered;
            Controller.PopUpController.CloseAll();
        }

        protected override void OnItemClicked(ItemActionType actionType, MarketItemModel model)
        {
            if (actionType == ItemActionType.CreateSellOffer)
            {
                if (WidgetModel.CurrentItemType == ItemModelType.GameInventory)
                {
                    MoveToMarketAndSell();
                }
                else
                {
                    ApplyCreateSellOfferCommand(model);
                }
            }
            else if (actionType == ItemActionType.EditSellOffer)
            {
                if (WidgetModel.CurrentItemType == ItemModelType.MySellOffers)
                {
                    ApplyEditSellOfferCommand(model);
                }
            }
            else
            {
                base.OnItemClicked(actionType, model);
            }
        }

        private void ApplyEditSellOfferCommand(MarketItemModel model)
        {
            var apiCommand = Strategy.GetEditSellOfferCommand(model);
            apiCommand.CommandFinished += OnSellItemFinished;
            SendApiCommand(apiCommand);
        }

        private void MoveToMarketAndSell()
        {
            var apiCommand = Strategy.GetChangeItemInventoryCommand(FormModel.CurrentItem, MarketMoveItemType.ToMarket);

            apiCommand.CommandFinished += OnMoveItemFinished;
            SendApiCommand(apiCommand);
        }

        private void OnMoveItemFinished(CommandBase command)
        {
            command.CommandFinished -= OnMoveItemFinished;

            if (command.Result)
            {
                ApplyCreateSellOfferCommand(FormModel.CurrentItem);
            }
        }

        private void ApplyCreateSellOfferCommand(MarketItemModel model)
        {
            var apiCommand = Strategy.GetCreateSellOfferCommand(model);
            apiCommand.CommandFinished += OnSellItemFinished;
            SendApiCommand(apiCommand);
        }

        private void OnSellItemFinished(CommandBase command)
        {
            command.CommandFinished -= OnSellItemFinished;
            if (command.Result)
            {
                var message = "Congratulations! Your {0} will appear in Dmarket inventory as soon as transaction" +
                    " will be processed. It may take up to 15 minutes";

                message = string.Format(message, FormModel.CurrentItem.Tittle);
                ShowSimpleNotification(message);

                ApplyState<MySellOffersState>();
            }
        }

        private void OnPriceEntered(long price)
        {
            FormModel.CurrentItem.Price.Amount = price;
            FormModel.SetChanges();
        }

        private void OnFormClosed()
        {
            ApplyPreviousState();
        }

        private void ShowCannotCreateNotification()
        {
            const string kMessage = "Note that after creating the sell offer with " +
                "{0}, it will disappear from you Game Inventory";
            var message = string.Format(kMessage, FormModel.CurrentItem.Tittle);
            ShowSimpleNotification(message);
        }
    }
}