using DMarketSDK.Market.Domain;
using DMarketSDK.Market.GameIntegration;
using UnityEngine;

namespace DMarketSDK.Market.Forms
{
    public sealed class GameInventoryState : MarketItemsStateBase<GameInventoryForm, ItemsFormModel> 
    {
        protected override MarketTabType MarketTab { get { return MarketTabType.InGameInventory; } }

        private IGameIntegrationModel GameModel { get { return Controller.GameModel; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            GameModel.ItemsChanged += OnGameModelChanged;
            LoadFormItems();

            //TODO need refactoring
            //TODO refactoring != delete
            if (Controller.IsNeedBlockMarket)
            {
                Debug.LogWarning("Inventory in Market widget is blocking for that platform and Market will be closed." +
                                 " For that platform recommended to use Basic widget");
                Controller.Close();
            }
        }

        public override void Finish()
        {
            base.Finish();
            GameModel.ItemsChanged -= OnGameModelChanged;
        }

        private void OnGameModelChanged()
        {
            LoadFormItems();
            WidgetModel.SetChanges();
        }
    }
}