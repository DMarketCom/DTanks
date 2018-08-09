using System;
using System.Collections.Generic;

namespace TankGame.Inventory.Domain
{
    public class ItemChangingRequest
    {
        public readonly ItemActionType ActionType;
        public readonly List<long> WorldIds;
        public readonly Action<ItemsChangingResponse> Callback;

        public ItemChangingRequest(ItemActionType actionType, List<long> worldIds,
            Action<ItemsChangingResponse> callback)
        {
            ActionType = actionType;
            WorldIds = worldIds;
            Callback = callback;
        }
    }
}