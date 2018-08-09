using DMarketSDK.Forms;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;
using SHLibrary.Logging;
using ShowDocumentFormModel = DMarketSDK.Forms.ShowDocumentFormModel;

namespace DMarketSDK.Basic.States
{
    public abstract class ShowDocumentStateBase<TFormView, TFormModel> : BasicWidgetFormStateBase<TFormView, TFormModel>
        where TFormView : ShowDocumentFormBase<TFormModel>
        where TFormModel : ShowDocumentFormModel, new()
    {
        private string Type { get { return FormModel.Type; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.CloseClicked += OnCloseClicked;
            LoadDocument();
        }

        public override void Finish()
        {
            base.Finish();

            View.CloseClicked -= OnCloseClicked;
        }

        protected virtual void OnCloseClicked()
        {
        }

        private void LoadDocument()
        {
            Controller.MarketApi.GetDocument(WidgetModel.BasicAccessToken, Type, OnDocumentLoadedCallback, OnErrorCallback);
            View.SetLoadingState(true);
        }

        private void OnDocumentLoadedCallback(GetDocumentRequest.Response response, GetDocumentRequest.RequestParams request)
        {
            FormModel.Text = response.body;
            FormModel.SetChanges();

            View.SetLoadingState(false);
        }

        private void OnErrorCallback(Error error)
        {
            DevLogger.Error(string.Format("Can't load {0} from API", Type));

            FormModel.Text = string.Format("Load text error:\n{0}", Type);
            View.SetLoadingState(false);
        }
    }
}