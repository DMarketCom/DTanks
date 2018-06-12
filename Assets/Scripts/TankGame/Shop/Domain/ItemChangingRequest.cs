using System;

namespace Shop.Domain
{
    public class ItemChangingRequest
    {
        public readonly ItemActionType ActionType;
        public readonly long WorldId;
        public readonly Action<ItemsChangingResponse> Callback;

        public ItemChangingRequest(ItemActionType actionType, long worldId,
            Action<ItemsChangingResponse> callback)
        {
            ActionType = actionType;
            WorldId = worldId;
            Callback = callback;
        }
    }
}