using System.Collections.Generic;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Commands.API
{
    public class ChangeInventoryForItemCommand : ApiItemOperationCommandBase
    {
        private readonly MarketMoveItemRequestParams _request;
        
        public ChangeInventoryForItemCommand(MarketItemModel itemModel, MarketMoveItemType marketMoveType) : base(itemModel)
        {
            _request = new MarketMoveItemRequestParams
            {
                AssetIds = new List<string>() { TargetItemModel.AssetId },
                ClassIds = new List<string>() { TargetItemModel.ClassId },
                TransactionType = marketMoveType,
                Callback = OnGameResponse
            };
        }

        public override void Start()
        {
            base.Start();
            Controller.MoveItemAcrossInventory(_request);
        }

        private void OnGameResponse(MarketMoveItemResponse response)
        {
            if(!response.IsSuccess)
            {
                OnError(response.Error, response.ErrorCode);
            }

            Terminate(response.IsSuccess);
        }
    }
}