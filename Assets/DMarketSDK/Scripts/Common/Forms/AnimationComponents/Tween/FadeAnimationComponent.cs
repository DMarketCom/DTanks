using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Common.Forms.AnimationComponents
{
    public class FadeAnimationComponent : TweenAnimationComponentBase
    {
        private CanvasGroup _canvasGroup;

        public FadeAnimationComponent(TweenAnimParameters tweenParameters) : base(tweenParameters)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _canvasGroup = Target.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = Target.AddComponent<CanvasGroup>();
            }
            _canvasGroup.alpha = 0f;
        }

        protected override Tweener PlayBaseAnim(bool forShow)
        {
            return _canvasGroup.DOFade(forShow ? 1 : 0, AnimTime);
        }
    }
}