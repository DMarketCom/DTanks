using System;

namespace PlayerData
{
    [Serializable]
    public class PlayerAutorizationInfo
    {
        public string UserName;
        public string Password;

        public bool IsLogged { get { return UserName != string.Empty; } }

        public PlayerAutorizationInfo() : this(string.Empty, string.Empty)
        { }

        public PlayerAutorizationInfo(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
