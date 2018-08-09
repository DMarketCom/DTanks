using DMarketSDK.Forms;

namespace DMarketSDK.Market.Forms
{
    public sealed class TermsOfUseState : ShowDocumentStateBase<TermsOfUseForm, ShowDocumentFormModel>
    {
        protected override void OnCloseClicked()
        {
            base.OnCloseClicked();

            ApplyPreviousState();
        }
    }
}