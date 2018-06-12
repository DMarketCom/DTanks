using Networking.Msg;
using SHLibrary.ObserverView;
using System.Collections.Generic;

namespace GameServer
{
    public class ServerModel : ObservableBase
    {
        public readonly Dictionary<int, string> ConIdToUserName = 
            new Dictionary<int, string>();

        public readonly Dictionary<int, GameBattlePlayerInfo> UnitsInBattle =
          new Dictionary<int, GameBattlePlayerInfo>();

        private readonly List<DropItemInfo> _itemsInField = new List<DropItemInfo>();

        public string MarketToken;
        public string MakretRefreshToken;

        public List<DropItemInfo> ItemsInField { get { return _itemsInField; } }

        public void RemoveItem(long worldId)
        {
            var target = _itemsInField.Find(item => item.WorldId == worldId);
            _itemsInField.Remove(target);
        }

        public void AddItem(DropItemInfo dropInfo)
        {
            _itemsInField.Add(dropInfo);
        }
    }
}
