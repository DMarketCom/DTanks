using System;

namespace PlayerData
{
    [Serializable]
    public class PlayerInfo
    {
		public int Version = 2;
        public PlayerAutorizationInfo Autorziation;
        public PlayerInventoryInfo Inventory;

        public PlayerInfo()
        {
            Autorziation = new PlayerAutorizationInfo();
            Inventory = new PlayerInventoryInfo();
        }
    }
}