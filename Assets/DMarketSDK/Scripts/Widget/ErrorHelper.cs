using DMarketSDK.IntegrationAPI;
using System.Collections.Generic;

namespace DMarketSDK.Widget
{
    public static class ErrorHelper
    {
        public const string EmptyPasswordMessage = "Password can't be empty";
        public const string EmptyEmailMessage = "Email can't be empty";
        public const string EmptyLoginMessage = "Login can`t be empty";
        public const string WrongEmailPatternMessage = "Wrong email pattern";

        //TODO move to scriptable object or exel
        private static readonly Dictionary<ErrorCode, string> _errorsMessages;

        static ErrorHelper()
        {
            _errorsMessages = new Dictionary<ErrorCode, string>();
        }

        public static string GetErrorMessage(ErrorCode errorCode)
        {
            return _errorsMessages.ContainsKey(errorCode) ? _errorsMessages[errorCode] : GetDefaultErrorMessage(errorCode);
        }

        private static string GetDefaultErrorMessage(ErrorCode errorCode)
        {
            return "An error occured: " + errorCode;
        }
    }
}