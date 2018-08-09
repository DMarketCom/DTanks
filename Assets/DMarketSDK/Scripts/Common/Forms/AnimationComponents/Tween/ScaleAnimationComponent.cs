using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Common.Forms.AnimationComponents
{
    public class ScaleAnimationComponent : TweenAnimationComponentBase
    {
        public ScaleAnimationComponent(TweenAnimParameters tweenParameters) : base(tweenParameters)
        {
        }

        protected override Tweener PlayBaseAnim(bool forShow)
        {
            var endValue = forShow ? Vector3.one : Vector3.zero;
            return TransformAnchor.transform.DOScale(endValue, AnimTime);
        }
    }
}