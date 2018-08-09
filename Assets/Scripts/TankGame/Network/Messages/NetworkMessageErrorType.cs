using System.Collections.Generic;

namespace TankGame.Network.Messages
{
    public enum NetworkMessageErrorType : short
    {
        None = 0,
        UserNameNotRegister,
        UserNameBusy,
        UserNameCannotBeEmpty,
        UserPasswordNotCorrect,
        UserPasswordCannotBeEmpty,
        DMarketError
    }

    public static class NetworkMessagesInfo
    {
        private static readonly Dictionary<NetworkMessageErrorType, string> Messages = new Dictionary<NetworkMessageErrorType, string>
        {
            {NetworkMessageErrorType.None, ""},
            {NetworkMessageErrorType.UserNameNotRegister, "Username does not exist"},
            {NetworkMessageErrorType.UserNameBusy, "Username is already in use"},
            {NetworkMessageErrorType.UserNameCannotBeEmpty, "Username can't be empty"},
            {NetworkMessageErrorType.UserPasswordNotCorrect, "Password has not correct"},
            {NetworkMessageErrorType.UserPasswordCannotBeEmpty, "Password can't be empty"},
            {NetworkMessageErrorType.DMarketError, "DMarket error"}
        };

        public static string GetMessage(NetworkMessageErrorType type)
        {
            string message;
            Messages.TryGetValue(type, out message);
            return message;
        }
    }
}