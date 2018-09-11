using System;
using DMarketSDK.Common.UI;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Forms
{
    public abstract class ShowDocumentFormBase<T> : WidgetFormViewBase<T>
        where T : ShowDocumentFormModel
    {
        public event Action CloseClicked;

        [SerializeField]
        private string _type;

        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private Button _closeButton;
        [SerializeField]
        private LoadingSpinner _loadingSpinner;

        public override void ApplyModel(WidgetFormModel model)
        {
            base.ApplyModel(model);
            FormModel.Type = _type;
        }

        protected override void OnModelChanged()
        {
            //TODO string.IsNullOrEmpty is tmp WebGl loading fix
            if (!string.IsNullOrEmpty(FormModel.Text))
            {
                _text.text = FormModel.Text;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked()
        {
            CloseClicked.SafeRaise();
        }

        public void SetLoadingState(bool isLoading)
        {
            _loadingSpinner.SetActiveSpinner(isLoading);
        }
    }
}