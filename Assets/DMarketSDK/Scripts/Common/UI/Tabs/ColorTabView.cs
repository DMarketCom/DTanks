using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace DMarketSDK.Common.UI
{
    public class ColorTabView : TabViewBase
    {
        [SerializeField]
        private Button _btnTab;
        [SerializeField]
        private Color _selectedColor;
        [SerializeField]
        private Color _deselectedColor;
        [SerializeField]
        private Ease _easeType;
        [SerializeField]
        private float _animTime;
        [SerializeField]
        private TextMeshProUGUI _titleText;
        [SerializeField]
        private List<GameObject> _lstSelectedObjects;
        [SerializeField]
        private List<GameObject> _lstDeselectedObjects;

        private bool _isSelected;
        private Tweener _currentAnim;

        public override bool IsSelected
        {
            get
            {
                return _isSelected;
            }
        }

        public override bool Interactable
        {
            set
            {
                _btnTab.interactable = value;
            }
        }

        public override string Title
        {
            set
            {
                _titleText.text = value;
            }
        }

        public override void SetState(bool isSelected, bool useAnimation)
        {
            _isSelected = isSelected;
            if (_currentAnim != null)
            {
                _currentAnim.Kill(false);
            }
            var targetColor = _isSelected ? _selectedColor : _deselectedColor;
            if (useAnimation)
            {
                _currentAnim = _btnTab.image.DOColor(targetColor, _animTime);
                _currentAnim.SetEase(_easeType);
            }
            else
            {
                _btnTab.image.color = targetColor;
            }
            _lstDeselectedObjects.ForEach(item => item.SetActive(!IsSelected));
            _lstSelectedObjects.ForEach(item => item.SetActive(IsSelected));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnTab.onClick.AddListener(OnTabClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnTab.onClick.RemoveListener(OnTabClicked);
        }

        private void OnTabClicked()
        {
            Clicked.SafeRaise(this);
        }
    }
}