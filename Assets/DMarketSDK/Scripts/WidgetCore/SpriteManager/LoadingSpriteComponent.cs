using System;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.WidgetCore.SpriteManager
{
    public class LoadingSpriteComponent
    {
        public event Action Updated;

        private readonly ISpriteItemContainer _container;

        private string _currentUrl;
        private Image _image;

        public LoadingSpriteComponent(ISpriteItemContainer container)
        {
            _container = container;
        }

        public void ApplySprite(string url, Image image)
        {
            UnloadPreviousSprite();
            _image = image;
            _currentUrl = url;
            _container.SpriteUpdated += OnSpriteUpdated;
            var targetSprite = _container.GetSprite(_currentUrl);
            OnSpriteUpdated(_currentUrl, targetSprite);
        }

        public void UnloadPreviousSprite()
        {
            if (!string.IsNullOrEmpty(_currentUrl))
            {
                _image.sprite = null;
                _container.ReturnSprite(_currentUrl);
                _container.SpriteUpdated -= OnSpriteUpdated;
            }
        }

        private void OnSpriteUpdated(string url, Sprite sprite)
        {
            if (_currentUrl == url)
            {
                _image.sprite = sprite;
            }
            Updated.SafeRaise();
        }
    }
}