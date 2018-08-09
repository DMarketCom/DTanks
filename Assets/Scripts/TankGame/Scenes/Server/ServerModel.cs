using System.Collections.Generic;
using System.Linq;
using SHLibrary.ObserverView;
using TankGame.Network.Messages;

namespace TankGame.GameServer
{
    public class ServerModel : ObservableBase
    {
        public readonly Dictionary<int, GameBattlePlayerInfo> UnitsInBattle = new Dictionary<int, GameBattlePlayerInfo>();
        private readonly HashSet<UserSessionInfo> _registeredUserSessions = new HashSet<UserSessionInfo>();
        private readonly Dictionary<int, string> _playersMarketAccessTokens = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _playerMarketRefreshTokens = new Dictionary<int, string>();
        private readonly List<DropItemInfo> _itemsInField = new List<DropItemInfo>();

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

        public bool SpawnPointIsEmpty(int itemSpawnPointIndex)
        {
            foreach(DropItemInfo item in _itemsInField)
            {
                if(item.ItemSpawnPointIndex == itemSpawnPointIndex)
                {
                    return false;
                }
            }
            return true;
        }

        public string GetPlayerMarketAccessToken(int connectionId)
        {
            string accessToken;
            _playersMarketAccessTokens.TryGetValue(connectionId, out accessToken);
            return accessToken;
        }

        public string GetPlayerMarketRefreshToken(int connectionId)
        {
            string refreshToken;
            _playerMarketRefreshTokens.TryGetValue(connectionId, out refreshToken);
            return refreshToken;
        }

        public void SetPlayerMarketAccessToken(int connectionId, string marketAccessToken)
        {
            if (_playersMarketAccessTokens.ContainsKey(connectionId))
            {
                _playersMarketAccessTokens[connectionId] = marketAccessToken;
                return;
            }

            _playersMarketAccessTokens.Add(connectionId, marketAccessToken);
        }

        public void SetPlayerMarketRefreshToken(int connectionId, string marketRefreshToken)
        {
            if (_playerMarketRefreshTokens.ContainsKey(connectionId))
            {
                _playerMarketRefreshTokens[connectionId] = marketRefreshToken;
                return;
            }

            _playerMarketRefreshTokens.Add(connectionId, marketRefreshToken);
        }

        public void RemovePlayerMarketAccessToken(int connectionId)
        {
            _playersMarketAccessTokens.Remove(connectionId);
        }

        public void RemovePlayerMarketRefreshToken(int connectionId)
        {
            _playerMarketRefreshTokens.Remove(connectionId);
        }

        public void AddUserSession(int connectionId, string userName)
        {
            _registeredUserSessions.Add(new UserSessionInfo(connectionId, userName));
            SetChanges();
        }

        public void RemoveUserSession(int connectionId)
        {
            _registeredUserSessions.RemoveWhere(c => c.ConnectionId == connectionId);
            SetChanges();
        }

        public int GetConnectionIdByUserName(string userName)
        {
            return _registeredUserSessions.First(c => c.UserName == userName).ConnectionId;
        }

        public string GetUserNameByConnectionId(int connectionId)
        {
            return _registeredUserSessions.First(c => c.ConnectionId == connectionId).UserName;
        }

        public bool IsUserSessionActive(int connectionId)
        {
            return _registeredUserSessions.Any(c => c.ConnectionId == connectionId);
        }

        private struct UserSessionInfo
        {
            public readonly int ConnectionId;
            public readonly string UserName;

            public UserSessionInfo(int connectionId, string userName)
            {
                ConnectionId = connectionId;
                UserName = userName;
            }
        }
    }
}
