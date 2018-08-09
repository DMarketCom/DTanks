using DMarketSDK.Common.Sound;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using DMarketSDK.Market.States;
using System.Text.RegularExpressions;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Market.Forms
{
    public abstract class MarketFormStateBase<TFormView, TFormModel> : MarketApiStateBase 
        where TFormView : WidgetFormViewBase<TFormModel> 
        where TFormModel : WidgetFormModel, new ()
    {
        protected TFormView View { get; private set; }
        protected TFormModel FormModel { get { return View.FormModel; } }

        protected virtual MarketTabType MarketTab { get { return MarketTabType.None; } }

        protected virtual bool IsBlockTabPanel { private get; set; }

        protected virtual bool IsNeedChangeTabOnStart { get { return true; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            View = Controller.GetForm<TFormView>();
            if (View.Model == null)
            {
                View.ApplyModel(new TFormModel());
            }
            View.Show();

            if (IsNeedChangeTabOnStart)
            {
                MarketView.ApplyActivePanelTab(MarketTab);
            }
            MarketView.PanelTabChanged += OnTabChanged;

            IFormWithItems formWithItems = View as IFormWithItems;
            if (formWithItems != null)
            {
                formWithItems.ItemClicked += OnItemClicked;
            }
        }

        public override void Finish()
        {
            base.Finish();
            View.Hide();
            MarketView.PanelTabChanged -= OnTabChanged;

            IFormWithItems formWithItems = View as IFormWithItems;
            if (formWithItems != null)
            {
                formWithItems.ItemClicked -= OnItemClicked;
            }
        }

        protected virtual void OnItemClicked(ItemActionType actionType, MarketItemModel itemModel)
        {
            switch (actionType)
            {
                case ItemActionType.SellOnDmarket:
                    ApplySellItemState(itemModel);
                    break;
                case ItemActionType.Buy:
                    ApplyCheckoutItemState(itemModel);
                    break;
				case ItemActionType.BuyItem:
					ApplyBuyItemState(itemModel);
					break;
            }
        }

        protected bool IsEmailFormat(string email)
        {
            const string MatchEmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            if (!Regex.IsMatch(email, MatchEmailPattern))
            {
                ShowError(ErrorCode.WrongEmailPattern);
                return false;
            }
            return true;
        }

        protected virtual void ShowError(ErrorCode errorCode)
        {
        }

        private void ApplySellItemState(MarketItemModel model)
        {
            ApplyState<MarketSellOfferState>(model, ItemActionType.CreateSellOffer);
        }

		private void ApplyBuyItemState(MarketItemModel model)
		{
            ApplyState<BuyClassItemState>(model);
		}

        private void ApplyCheckoutItemState(MarketItemModel model)
        {
            ApplyState<CheckoutItemState>(model);
        }

        private void OnTabChanged(MarketTabType tabType)
        {
            if (IsBlockTabPanel)
            {
                MarketView.ApplyActivePanelTab(MarketTab);
                return;
            }
            Controller.SoundManager.Play(MarketSoundType.SwitchForm);
            ApplyTabState(tabType);
        }

        private void ApplyTabState(MarketTabType tabType)
        {
            switch (tabType)
            {
                case MarketTabType.InGameInventory:
                    ApplyState<GameInventoryState>();
                    break;
                case MarketTabType.DmarketInventory:
                    ApplyState<DMarketInventoryState>();
                    break;
                case MarketTabType.MySellOffers:
                    ApplyState<MySellOffersState>();
                    break;
                case MarketTabType.BuyItems:
                    ApplyState<BuyItemsState>();
                    break;
            }
        }

        private void ApplyDefaultIdleState()
        {
            var targetTab = MarketTabType.InGameInventory;
            switch (WidgetModel.CurrentItemType)
            {
                case ItemModelType.GameInventory:
                    targetTab = MarketTabType.InGameInventory;
                    break;
                case ItemModelType.MarketInventory:
                    targetTab = MarketTabType.DmarketInventory;
                    break;
                case ItemModelType.MySellOffers:
                    targetTab = MarketTabType.MySellOffers;
                    break;
                case ItemModelType.BuyItems:
                    targetTab = MarketTabType.BuyItems;
                    break;
            }
            ApplyTabState(targetTab);
        }
    }
}