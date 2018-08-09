using System;

namespace TankGame.Domain.GameItem
{
    [Serializable]
    public class GameItemInfo
    {
        public string Name;
        public string Description;

        public GameItemInfo()
        {
            Name = "Unknow";
            Description = string.Empty;
        }

        public GameItemInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
