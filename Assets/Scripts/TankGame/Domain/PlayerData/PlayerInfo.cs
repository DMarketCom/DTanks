using System;

namespace TankGame.Domain.PlayerData
{
    [Serializable]
    public class PlayerInfo
    {
		public int Version = 2;
        public PlayerAuthInfo AuthInfo;
        public PlayerInventoryInfo Inventory;

        public PlayerInfo()
        {
            AuthInfo = new PlayerAuthInfo();
            Inventory = new PlayerInventoryInfo();
        }
    }
}