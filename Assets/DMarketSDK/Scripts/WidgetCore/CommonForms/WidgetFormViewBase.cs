using DMarketSDK.Common.Forms;
using DMarketSDK.Domain;
using UnityEngine;

namespace DMarketSDK.WidgetCore.Forms
{
    public enum MarketFormLayerType
    {
        Low = 10,
        Middle = 20,
        High = 30
    }

    public abstract class WidgetFormViewBase<TModel> : WidgetFormViewBase where TModel : WidgetFormModel
    {
        private TModel _formModel;

        public TModel FormModel { get { return _formModel; } }

        public override void ApplyModel(WidgetFormModel model)
        {
            _formModel = (TModel)model;
            base.ApplyModel(model);
        }
    }

    public abstract class WidgetFormViewBase : AnimObserverFormBase<WidgetFormModel>
    {
        [SerializeField]
        private MarketFormLayerType _layer = MarketFormLayerType.Low;

        public MarketFormLayerType Layer { get { return _layer; } }

        public void SetLayer(RectTransform parent)
        {
            GetComponent<RectTransform>().SetParent(parent, false);
        }

        protected override void OnModelChanged() { }
    }
}