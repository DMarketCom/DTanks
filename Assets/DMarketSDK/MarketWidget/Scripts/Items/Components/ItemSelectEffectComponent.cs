using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Items.Components
{
    [RequireComponent(typeof(Image))]
    public class ItemSelectEffectComponent : ItemSelectComponentBase
    {
        private Image _img;
        private Tweener _tweener;
        private Color _defaultColor;

        protected override bool IsShowing
        {
            get { return _tweener != null; }
        }

        public override void ApplyItem(MarketItemView item)
        {
            _img = GetComponent<Image>();
            _defaultColor = _img.color;
            base.ApplyItem(item);
        }

        public override void RemoveItem()
        {
            base.RemoveItem();
            DestroyTween();
        }

        private void DestroyTween()
        {
            if (_tweener != null)
            {
                _tweener.Kill(false);
                _tweener = null;
            }
        }
        
        protected override void SetState(bool value)
        {
            _img.enabled = value;
            DestroyTween();
            if (value)
            {
                _img.color = _defaultColor;
                _tweener = _img.DOFade(0.4f, 3f);
                _tweener.SetLoops(100, LoopType.Yoyo);
                _tweener.SetEase(Ease.InSine);
            }
        }
    }
}