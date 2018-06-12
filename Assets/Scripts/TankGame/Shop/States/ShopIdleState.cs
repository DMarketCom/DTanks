using Shop.DMarketIntegration.States;
using SHLibrary.Utils;
using Shop.Domain;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Forms;

namespace Shop.States
{
    public class ShopIdleState : ShopStateBase
    {
        private readonly Dictionary<ShopItemClickType, ItemActionType> _sendToServerClicks = new Dictionary<ShopItemClickType, ItemActionType>
        {
            {ShopItemClickType.Buy, ItemActionType.Buy},
            {ShopItemClickType.Sell, ItemActionType.Sell},
            {ShopItemClickType.FromMarket, ItemActionType.FromMarket},
            {ShopItemClickType.ToMarket, ItemActionType.ToMarket},
            {ShopItemClickType.Equip, ItemActionType.Equip}
        };

        private MessageBoxForm MessageBox { get { return View.MessageBoxForm; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            Model.Mode = Controller.Widget.IsLogged ?
                ShopModeType.ShowDMarketItems : ShopModeType.NotShowDmarketItems;
            View.ApplyModel(Model);
            View.Show();
            View.ShowBasicWidgetButton();
            View.ItemEvent += OnItemEvent;
            View.BackClicked += OnBackClick;
            View.BasicWidgetClicked += OnBasicWidgetClicked;
        }

        public override void Finish()
        {
            base.Finish();
            //View.Hide();
            View.ItemEvent -= OnItemEvent;
            View.BackClicked -= OnBackClick;
            View.BasicWidgetClicked -= OnBasicWidgetClicked;
        }

        private void OnItemEvent(ShopItemClickType clickType, long worldId)
        {
            bool isUseMarket = clickType == ShopItemClickType.ToMarket || clickType == ShopItemClickType.FromMarket;
            if (isUseMarket && !Controller.Widget.IsLogged)
            {
                ApplyState<ShopOpenBasicWidgetState>();
                return;
            }

            if (clickType == ShopItemClickType.LoginToMarket)
            {
                ApplyState<ShopOpenBasicWidgetState>();
            }
            else if(_sendToServerClicks.ContainsKey(clickType))
            {
                SendToServer(_sendToServerClicks[clickType], worldId);
            }
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            Controller.BackClicked.SafeRaise();
        }

        private void OnBasicWidgetClicked()
        {
            //View.Hide();
            View.HideBasicWidgetButton();
            if (Controller.Widget.IsInitialize)
            {
                ApplyState<ShopOpenBasicWidgetState>();
            }
            else
            {
                ApplyState<ShopBasicWidgetInitializeState>();
            }
        }

        private void SendToServer(ItemActionType actionType, long worldId)
        {
            var message = new ItemChangingRequest(actionType, worldId,
                OnServerCallback);
            Controller.MakedItemAction(message);
            View.WaitingForm.Show();
        }
 
        protected virtual void OnServerCallback(ItemsChangingResponse parameters)
        {
            View.WaitingForm.Hide();
            if (parameters.IsSuccess)
            {
                //TODO change to notification
                DevLogger.Warning("Add notification here");
                //FormView.ShowSuccesOperation();
            }
            else
            {
                MessageBox.Show("Error", parameters.ErrorText);
            }
        }
    }
}