using System;
using System.Collections.Generic;
using DMarketSDK.Market.Commands.API;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using SHLibrary.Logging;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.Forms
{
    public abstract class MarketItemsStateBase<TForm, TModel> : MarketFormStateBase<TForm, TModel>
        where TForm : TableFormBase<TModel>
        where TModel : ItemsFormModel, new()
    {
        private readonly Dictionary<ItemModelType, Action<ShowingItemsInfo>> _itemLoaders;

        protected ItemModelType TargetItemType
        {
            get { return View.ItemType; }
        }

        protected MarketItemsContainer ContainerView
        {
            get { return View.Container; }
        }

        protected MarketItemsStateBase()
        {
            _itemLoaders = new Dictionary<ItemModelType, Action<ShowingItemsInfo>>
            {
                {ItemModelType.GameInventory, LoadGameInventory},
                {ItemModelType.MarketInventory, LoadMarketInventory},
                {ItemModelType.MySellOffers, LoadMySellOrders},
                {ItemModelType.BuyItems, LoadBuyItems},
                {ItemModelType.BuyItem, LoadBuyItemsByClass}
            };
        }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            ContainerView.UpdateShowingItemsInfo += OnShowingItemsInfoUpdated;

            if (ContainerView.Model == null)
            {
                ContainerView.ApplyModel(new MarketContainerModel {ShowingItemsInfo = View.FormModel.ShowingItemsInfo});
            }

            WidgetModel.CurrentItemType = TargetItemType;
            WidgetModel.SetChanges();

            //TODO Roma: need save showing items for every table using models
            var showingItemsInfo = new ShowingItemsInfo(0, View.ItemPerPage);
            FormModel.ShowingItemsInfo = showingItemsInfo;
            FormModel.SetChanges();

            var containerModel = ContainerView.Model;
            containerModel.ShowingItemsInfo = showingItemsInfo;
            containerModel.SetChanges();


            SendApiCommand(Strategy.GetUpdateBalanceCommand(), false);
        }

        public override void Finish()
        {
            base.Finish();

            ContainerView.Model.Clear();
            ContainerView.Model.SetChanges();
            ContainerView.UpdateShowingItemsInfo -= OnShowingItemsInfoUpdated;
        }

        protected override void OnItemClicked(ItemActionType actionType, MarketItemModel itemModel)
        {
            base.OnItemClicked(actionType, itemModel);

            switch (actionType)
            {
                case ItemActionType.Select:
                    OnItemSelected(itemModel);
                    break;

                case ItemActionType.SendToDmarket:
                    ApplyChangeItemInventoryCommand(itemModel, MarketMoveItemType.ToMarket);
                    break;

                case ItemActionType.SendToGameInventory:
                    ApplyChangeItemInventoryCommand(itemModel, MarketMoveItemType.FromMarket);
                    break;

                case ItemActionType.CancelSellOffer:
                    ApplyCancelSellOfferCommand(itemModel);
                    break;
                case ItemActionType.EditSellOffer:
                    OnEditSellOffer(itemModel);
                    break;
            }
        }

        private void OnItemSelected(MarketItemModel model)
        {
            FormModel.SelectedItem = model;
            FormModel.SetChanges();
        }

        private void OnEditSellOffer(MarketItemModel model)
        {
            ApplyState<MarketSellOfferState>(model, ItemActionType.EditSellOffer);
        }

        private void ApplyChangeItemInventoryCommand(MarketItemModel model, MarketMoveItemType marketMoveType)
        {
            FormModel.AddItemLock(model);
            FormModel.SetChanges();
            var apiCommand = Strategy.GetChangeItemInventoryCommand(model, marketMoveType);
            apiCommand.CommandFinished += OnChangeInventoryItemFinished;
            SendApiCommand(apiCommand, false);
        }

        private void ApplyCancelSellOfferCommand(MarketItemModel model)
        {
            var cancelOfferCommand = Strategy.GetCancelSellOfferCommand(model);
            cancelOfferCommand.CommandFinished += OnCancelSellOfferFinished;
            SendApiCommand(cancelOfferCommand, false);
        }

        private void OnCancelSellOfferFinished(CommandBase command)
        {
            LoadFormItems();
        }

        private void OnChangeInventoryItemFinished(CommandBase command)
        {
            var apiOperationCommand = command as ApiItemOperationCommandBase;
            //TODO make special changeInventory interface for correct work with mockup and online command
            if (apiOperationCommand != null)
            {
                FormModel.RemoveItemLock(apiOperationCommand.TargetItemModel);
                FormModel.SetChanges();
            }

            if (command.Result)
            {
                LoadFormItems();
            }
        }

        private void OnShowingItemsInfoUpdated(ShowingItemsInfo showingItemsInfo)
        {
            UpdateShowingItemsInfo(showingItemsInfo);
            LoadFormItems();
        }

        private void UpdateShowingItemsInfo(ShowingItemsInfo showingItemsInfo)
        {
            ContainerView.Model.ShowingItemsInfo = showingItemsInfo;
            ContainerView.Model.SetChanges();

            FormModel.ShowingItemsInfo = showingItemsInfo;
            FormModel.SetChanges();
        }

        protected virtual void LoadFormItems()
        {
            ShowingItemsInfo showingItemsInfo = FormModel.ShowingItemsInfo;
            ItemModelType modelType = WidgetModel.CurrentItemType;

            Action<ShowingItemsInfo> loadAction;
            if (!_itemLoaders.TryGetValue(modelType, out loadAction))
            {
                DevLogger.Error(string.Format("Need add loader for {0}", modelType));
                return;
            }

            loadAction.SafeRaise(showingItemsInfo);
        }

        private void LoadGameInventory(ShowingItemsInfo loadInfo)
        {
            var loadGameInventoryCommand = new LoadGameInventoryCommand(loadInfo);
            loadGameInventoryCommand.CommandFinished += OnLoadGameItemsFinished;
            ApplyCommand(loadGameInventoryCommand);
        }

        private void LoadMarketInventory(ShowingItemsInfo loadInfo)
        {
            var apiCommand = Strategy.GetLoadMarketInventoryCommand(loadInfo);
            apiCommand.CommandFinished += OnLoadMarketItemsFinished;
            SendLoadItemsCommand(apiCommand);
        }

        private void LoadMySellOrders(ShowingItemsInfo loadInfo)
        {
            var apiCommand = Strategy.GetLoadMySellOffersItemsCommand(loadInfo);
            apiCommand.CommandFinished += OnLoadMarketItemsFinished;
            SendLoadItemsCommand(apiCommand);
        }

        private void LoadBuyItems(ShowingItemsInfo loadInfo)
        {
            var apiCommand = Strategy.GetLoadMarketItemsCommand(loadInfo);
            apiCommand.CommandFinished += OnLoadMarketItemsFinished;
            SendLoadItemsCommand(apiCommand);
        }

        private void LoadBuyItemsByClass(ShowingItemsInfo loadInfo)
        {
            loadInfo.ClassId = FormModel.SelectedItem.ClassId;
            var apiCommand = Strategy.GetLoadSellOffersByClassIdCommand(loadInfo);
            apiCommand.CommandFinished += OnLoadMarketItemsFinished;
            SendLoadItemsCommand(apiCommand);
        }

        private void SendLoadItemsCommand(ApiCommandBase apiCommand)
        {
            SendApiCommand(apiCommand, false);
        }

        private void OnLoadGameItemsFinished(CommandBase command)
        {
            command.CommandFinished -= OnLoadGameItemsFinished;
            if (!command.Result)
            {
                return;
            }

            LoadMarketItems((command as ILoadMarketItemsCommand).CommandResult);
        }

        private void OnLoadMarketItemsFinished(CommandBase command)
        {
            command.CommandFinished -= OnLoadMarketItemsFinished;

            if (!command.Result)
            {
                return;
            }

            var loadMarketItemsCommand = (ILoadMarketItemsCommand)command;
            var commandResult = loadMarketItemsCommand.CommandResult;
            LoadMarketItems(commandResult);
        }

        private void LoadMarketItems(LoadMarketItemsCommandResult commandResult)
        {
            FormModel.SetItems(commandResult.MarketItems);
            FormModel.SetChanges();

            var containerModel = ContainerView.Model;
            containerModel.SetItems(FormModel.MarketItems, commandResult.TotalItemsCount);
            containerModel.SetChanges();
        }
    }
}