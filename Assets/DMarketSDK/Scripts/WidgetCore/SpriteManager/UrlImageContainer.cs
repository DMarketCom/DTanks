using SHLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using SHLibrary.Logging;
using UnityEngine;

namespace DMarketSDK.WidgetCore.SpriteManager
{
    public class UrlImageContainer : UnityBehaviourBase, ISpriteItemContainer
    {
        public static ISpriteItemContainer Instance { get; private set; }
        
        #region ISpriteItemContainer ipmlementation

        public event Action<string, Sprite> SpriteUpdated;

        Sprite ISpriteItemContainer.GetSprite(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return _defaultSprite;
            }
            if (!_loadedSprites.ContainsKey(url))
            {
                var newItem = new LoadedItemInfo
                {
                    Count = 0,
                    Sprite = _defaultSprite
                };

                _loadedSprites.Add(url, newItem);
                StartCoroutine(LoadSprite(url));
            }
            _loadedSprites[url].Count++;
            return _loadedSprites[url].Sprite;
        }

        void ISpriteItemContainer.ReturnSprite(string url)
        {
            if (_loadedSprites.ContainsKey(url))
            {
                _loadedSprites[url].Count--;
            }
        }

        #endregion

        [SerializeField]
        private Sprite _defaultSprite;

        private readonly Dictionary<string, LoadedItemInfo> _loadedSprites
            = new Dictionary<string, LoadedItemInfo>();
        
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        private IEnumerator LoadSprite(string url)
        {
            Sprite sprite = null;

            const int maxUploadAttempts = 5;
            int attemptsCounter = 0;

            while (attemptsCounter < maxUploadAttempts)
            {
                using (WWW loadSpriteWww = new WWW(url))
                {
                    yield return loadSpriteWww;

                    if (loadSpriteWww.texture != null)
                    {
                        Rect spriteRect = new Rect(0, 0, loadSpriteWww.texture.width, loadSpriteWww.texture.height);
                        Vector2 spritePivot = new Vector2(0, 0);

                        sprite = Sprite.Create(loadSpriteWww.texture, spriteRect, spritePivot);
                        _loadedSprites[url].Sprite = sprite;

                        break;
                    }
                }

                attemptsCounter++;
            }

            if (attemptsCounter >= maxUploadAttempts)
            {
                DevLogger.Warning(string.Format("Sprite not loaded by {0} attempts. Url:\n {1}", attemptsCounter, url));
            }

            SpriteUpdated.SafeRaise(url, sprite);
            yield return null;
        }

        private class LoadedItemInfo
        {
            public Sprite Sprite;
            public int Count;
        }
    }
}