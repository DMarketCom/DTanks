using UnityEngine;
using DG.Tweening;
using SHLibrary;

namespace DMarketSDK.Common.UI
{
    public sealed class LoadingSpinner : UnityBehaviourBase
    {
        [SerializeField]
        private RectTransform _spinnerImage;

        private Tweener _rotateTweener;

        protected override void OnDestroyObject()
        {
            base.OnDestroyObject();
            DestroyTween();
        }

        public void SetActiveSpinner(bool isActive)
        {
            gameObject.SetActive(isActive);
            DestroyTween();

            if (isActive)
            {
                _rotateTweener = _spinnerImage.DORotate(new Vector3(0, 0, -360), 2.0f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InOutCubic).SetLoops(int.MaxValue);
            }
        }

        private void DestroyTween()
        {
            if (_rotateTweener != null)
            {
                _rotateTweener.Kill(true);
                _rotateTweener = null;
            }
        }
    }
}