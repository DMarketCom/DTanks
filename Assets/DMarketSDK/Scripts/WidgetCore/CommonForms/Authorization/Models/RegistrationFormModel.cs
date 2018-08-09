using DMarketSDK.Domain;

namespace DMarketSDK.Forms
{
    public sealed class RegistrationFormModel : WidgetFormModel
    {
        public MarketRegistrationData RegistrationData;

        public RegistrationFormModel()
        {
            RegistrationData = new MarketRegistrationData();
        }
    }
}