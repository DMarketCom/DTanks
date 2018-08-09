using System;

namespace TankGame.Domain.PlayerData
{
    [Serializable]
    public class PlayerAuthInfo
    {
        public string UserName;
        public string Password;

        public bool IsLogged { get { return UserName != string.Empty; } }

        public PlayerAuthInfo() : this(string.Empty, string.Empty)
        { }

        public PlayerAuthInfo(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
