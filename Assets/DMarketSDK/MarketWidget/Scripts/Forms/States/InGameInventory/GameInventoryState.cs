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