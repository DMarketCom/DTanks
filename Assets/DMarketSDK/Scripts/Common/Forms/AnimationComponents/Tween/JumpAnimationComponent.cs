using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Common.Forms.AnimationComponents
{
    public class JumpAnimationComponent : TweenAnimationComponentBase
    {
        private Vector3 _showingPos;
        private Vector3 _hidingPos;

        public JumpAnimationComponent(Vector3 hidingPos, TweenAnimParametrs tweenParametrs) : base(tweenParametrs)
        {
            _hidingPos = hidingPos;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _showingPos = TransformAnchor.transform.position;
            TransformAnchor.transform.position = _hidingPos;
        }

        protected override Tweener PlayBaseAnim(bool forShow)
        {
            var endValue = forShow ? _showingPos : _hidingPos;
            return TransformAnchor.transform.DOMove(endValue, AnimTime);
        }
    }
}