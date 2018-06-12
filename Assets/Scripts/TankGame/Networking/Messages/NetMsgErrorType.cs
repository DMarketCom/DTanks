using System.Collections.Generic;

namespace Networking.Msg
{
    public enum NetMsgErrorType : short
    {
        None = 0,
        UserNameNotRegister,
        UserNameBusy,
        UserNameCannotBeEmpty,
        UserPasswordNotCorrect,
        UserPasswordCannotBeEmpty,
        DMarketError
    }

    //TODO: move thos code to scriptabel object
    static public class NetMsgErrorMessages
    {
        static public Dictionary<NetMsgErrorType, string> Messages = new Dictionary<NetMsgErrorType, string>
        {
            { NetMsgErrorType.None, ""},
            { NetMsgErrorType.UserNameNotRegister, "Username does not exist"},
            { NetMsgErrorType.UserNameBusy, "Username was busy"},
            { NetMsgErrorType.UserNameCannotBeEmpty, "Username can't be empty"},
            { NetMsgErrorType.UserPasswordNotCorrect, "Password has not correct"},
            { NetMsgErrorType.UserPasswordCannotBeEmpty, "Password can't be empty"},
            { NetMsgErrorType.DMarketError, "DMarket error"}

        };

        static public string GetMessage(NetMsgErrorType Type)
        {
            string Message = string.Empty;
            Messages.TryGetValue(Type, out Message);
            return Message;
        }
    }
}