using Networking.Msg;
using PlayerData;
using Shop.Domain;
using SHLibrary;
using UnityEngine;

namespace TankGame.DevInstruments.ServerMsg
{
    class DevAddNewItem : UnityBehaviourBase
    {
        [SerializeField]
        private bool _addButton;
        [SerializeField]
        private GameItemType _itemType;

        private void Update()
        {
            if (_addButton)
            {
                _addButton = false;
                var appController = FindObjectOfType<AppController>();
                var message = new AppChangingItemsMessage
                {
                    ActionType = ItemActionType.DevTestAdd,
                    ItemType = _itemType
                };
                appController.Client.Send(message);
            }
        }
        
    }
}
