using DMarketSDK.Domain;

namespace DMarketSDK.Forms
{
    public sealed class RegistrationFormModel : WidgetFormModel
    {
        public MarketRegistrationData RegistrationData;

        public readonly string PrivacyPolicyUrl = "https://dmarket.com/privacy-policy";
        public readonly string TermsOfUseUrl = "https://dmarket.com/terms-of-use";

        public RegistrationFormModel()
        {
            RegistrationData = new MarketRegistrationData();
        }
    }
}