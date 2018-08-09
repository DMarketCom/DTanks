using DMarketSDK.Domain;

namespace DMarketSDK.Forms
{
    public sealed class WidgetLoginFormModel : WidgetFormModel
    {
        public string UserLogin;
        public string UserPassword;

        public void Clear()
        {
            UserLogin = string.Empty;
            UserPassword = string.Empty;
        }
    }
}