using System.Collections.Generic;
using DMarketSDK.Basic;
using TankGame.Inventory.DMarketIntegration;
using TankGame.Inventory.Domain;
using TankGame.Inventory.Items;
using TankGame.UI.Forms;

namespace TankGame.Inventory.States
{
    public class InventoryIdleState : InventoryStateBase
    {
        private readonly Dictionary<InventoryItemClickType, ItemActionType> _sendToServerClicks = new Dictionary<InventoryItemClickType, ItemActionType>
        {
            {InventoryItemClickType.Buy, ItemActionType.Buy},
            {InventoryItemClickType.Sell, ItemActionType.Sell},
            {InventoryItemClickType.FromMarket, ItemActionType.FromMarket},
            {InventoryItemClickType.ToMarket, ItemActionType.ToMarket},
            {InventoryItemClickType.Equip, ItemActionType.Equip}
        };

        private IWidgetCore Widget { get { return Controller.Widget; } }

        private MessageBoxForm MessageBox { get { return View.MessageBoxForm; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            Model.Mode = Widget.IsLogged ?
                InventoryModeType.ShowDmarketItems : InventoryModeType.HideDmarketItems; // TODO: maybe need Model.SetChanges().
            View.ApplyModel(Model);
            View.Show();
            View.ItemEvent += OnItemEvent;
            View.BackClicked += OnBackClick;
            View.MarketWidgetClicked += OnMarketWidgetClicked;
        }

        public override void Finish()
        {
            base.Finish();
            View.ItemEvent -= OnItemEvent;
            View.BackClicked -= OnBackClick;
            View.MarketWidgetClicked -= OnMarketWidgetClicked;
        }

        private void OnMarketWidgetClicked()
        {
            OpenDMarket();
        }

        private void OnItemEvent(InventoryItemClickType clickType, long worldId)
        {
            bool isUseMarket = clickType == InventoryItemClickType.ToMarket || clickType == InventoryItemClickType.FromMarket;
            if ((isUseMarket  && !Widget.IsLogged) || clickType == InventoryItemClickType.LoginToMarket)
            {
                OpenDMarket();
            }
            else if(_sendToServerClicks.ContainsKey(clickType))
            {
                SendToServer(_sendToServerClicks[clickType], worldId);
            }
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            Controller.CloseInventory();
        }

        private void SendToServer(ItemActionType actionType, long worldId)
        {
            var message = new ItemChangingRequest(actionType, new List<long>() { worldId }, OnItemMoveCallback);
            Controller.MarketItemAction(message);
            View.WaitingForm.Show();
        }
 
        protected virtual void OnItemMoveCallback(ItemsChangingResponse parameters)
        {
            View.WaitingForm.Hide();
            if (parameters.IsSuccess)
            {
                return;
            }

            var error = string.IsNullOrEmpty(parameters.ErrorText)
                ? Widget.GetErrorMessage(parameters.MarketError)
                : parameters.ErrorText;
            MessageBox.Show("Error", error);
        }

        private void OpenDMarket()
        {
            if (Widget.IsInitialized)
            {
                if (Controller.IsBasicIntegration)
                {
                    ApplyState<InventoryBasicWidgetIdleState>();
                }
                else
                {
                    ApplyState<InventoryMarketWidgetIdleState>();
                }
            }
            else
            {
                ApplyState<InventoryMarketInitializeState>();
            }
        }
    }
}