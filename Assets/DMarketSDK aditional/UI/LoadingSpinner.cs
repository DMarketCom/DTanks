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

        public void StartRunning()
        {
            DestroyTween();
            gameObject.SetActive(true);
            _rotateTweener = _spinnerImage.DORotate(new Vector3(0, 0, -360), 2.0f, RotateMode.FastBeyond360);
            _rotateTweener.SetEase(Ease.InOutCubic).SetLoops(10);
        }
        
        public void StopRunning()
        {
            DestroyTween();
            gameObject.SetActive(false);
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