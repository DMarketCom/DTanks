using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Items.Components
{
    public sealed class DropdownPanel : ItemComponentBase
    {
        [SerializeField]
        private RectTransform _dropdownPanelTransform;
        [SerializeField]
        private Button _btnOpen;
        [SerializeField]
        private List<Button> _btnsClose;

      protected override void OnEnable()
        {
            base.OnEnable();
            SetPanelEnabling(false);
            _btnOpen.onClick.AddListener(OnActivateClicked);
            _btnsClose.ForEach(button => button.onClick.AddListener(OnChildButtonClicked));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnOpen.onClick.RemoveListener(OnActivateClicked);
            _btnsClose.ForEach(button => button.onClick.RemoveListener(OnChildButtonClicked));
        }

        private void OnChildButtonClicked()
        {
            SetPanelEnabling(false);
        }

        private void OnActivateClicked()
        {
            SetPanelEnabling(Target.Model.IsSelected);
        }

        public override void ModelUpdate()
        {
            base.ModelUpdate();
            if (!Target.Model.IsSelected)
            {
                SetPanelEnabling(false);
            }
            
        }

        private void SetPanelEnabling(bool isEnable)
        {
            _dropdownPanelTransform.gameObject.SetActive(isEnable);
        }
    }
}