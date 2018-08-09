using DMarketSDK.Forms;
using ShowDocumentFormModel = DMarketSDK.Forms.ShowDocumentFormModel;

namespace DMarketSDK.Basic.States
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