using DMarketSDK.Common.Forms.AnimationComponents;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Containers.Components
{
    public sealed class MobileSearchComponent : SearchContainerComponent
    {
        [SerializeField]
        private Button _showSearchButton;
        [SerializeField]
        private Button _closeSearchButton;
        [SerializeField]
        private RectTransform _inputFieldPanel;

        private IFormAnimationComponent _animationComponent;
        private bool _isShow;

        protected override void Awake()
        {
            base.Awake();
            _animationComponent = new FadeAnimationComponent(new TweenAnimParameters(_inputFieldPanel.gameObject));
            _animationComponent.Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _showSearchButton.onClick.AddListener(OnShowSearchClicked);
            _closeSearchButton.onClick.AddListener(OnCloseSearchClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _showSearchButton.onClick.RemoveListener(OnShowSearchClicked);
            _closeSearchButton.onClick.RemoveListener(OnCloseSearchClicked);
        }

        private void OnShowSearchClicked()
        {
            if (!_isShow)
            {
                ChangeButtonsState(true);
                _animationComponent.Show();
            }
        }

        private void OnCloseSearchClicked()
        {
            ChangeButtonsState(false);
            _animationComponent.Hide();

            ApplySearchInput(string.Empty);
        }

        private void ChangeButtonsState(bool isShow)
        {
            _isShow = isShow;
            _showSearchButton.gameObject.SetActive(!_isShow);
            _closeSearchButton.gameObject.SetActive(_isShow);
        }
    }
}