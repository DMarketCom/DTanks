using DMarketSDK.Forms;

namespace DMarketSDK.Market.Forms
{
    public sealed class PrivacyPolicyState : ShowDocumentStateBase<PrivacyPolicyForm, ShowDocumentFormModel>
    {
        protected override void OnCloseClicked()
        {
            base.OnCloseClicked();

            ApplyPreviousState();
        }
    }
}