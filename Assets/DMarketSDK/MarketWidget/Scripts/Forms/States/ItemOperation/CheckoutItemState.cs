using DMarketSDK.Market.Items;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.Forms
{
    public class CheckoutItemState : MarketFormStateBase<CheckoutItemForm, ItemInfoFormModel>
    {
        protected override bool IsNeedChangeTabOnStart
        {
            get
            {
                return false;
            }
        }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            
            View.Closed += OnFormClosed;

            FormModel.CurrentItem = (MarketItemModel)args[0];
            FormModel.SetChanges();
        }

        public override void Finish()
        {
            base.Finish();
            View.Closed -= OnFormClosed;
        }

        protected override void OnItemClicked(ItemActionType actionType, MarketItemModel model)
        {
            if (actionType == ItemActionType.Buy)
            {
                ApplyBuyItemCommand(model);
            }
            else
            {
                base.OnItemClicked(actionType, model);
            }
        }

        private void ApplyBuyItemCommand(MarketItemModel model)
        {
            var apiCommand = Strategy.GetBuyItemCommand(model);
            apiCommand.CommandFinished += OnCommandFinished;
            SendApiCommand(apiCommand);
        }

        private void OnCommandFinished(CommandBase command)
        {
            command.CommandFinished -= OnCommandFinished;
            if (command.Result)
            {
                ApplyState<BuyItemsState>();
            }
        }

        private void OnFormClosed()
        {
            ApplyPreviousState(FormModel.CurrentItem);
        }
    }
}