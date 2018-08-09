using System.Collections.Generic;
using System.Linq;
using TankGame.Domain.GameItem;
using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.DMarket.Domain
{
    public class MoveAssetTransactionInfo
    {
        public readonly AppChangingItemsMessage Sender;

        private readonly Dictionary<string, ItemInfo> _operations;

        public string[] OperationIds
        {
            get { return _operations.Keys.ToArray(); }
        }

        public int OperationsCount
        {
            get { return _operations.Count; }
        }

        public MoveAssetTransactionInfo(AppChangingItemsMessage sender)
        {
            Sender = sender;
            _operations = new Dictionary<string, ItemInfo>();
        }

        public void AddOperation(string operationId, ItemInfo itemInfo)
        {
            _operations.Add(operationId, itemInfo);
        }

        public void RemoveOperation(string operationId)
        {
            _operations.Remove(operationId);
        }

        public GameItemType GetItemType(string operationId)
        {
            return _operations[operationId].ItemType;
        }

        public long GetWorldId(string operationId)
        {
            return _operations[operationId].WorldId;
        }
    }
}
