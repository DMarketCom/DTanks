using SHLibrary;
using UnityEngine;
using DG.Tweening;

namespace DMarketSDK.Common.VisualEffects
{
    public class SimpleAnimShowingComponent : UnityBehaviourBase
    {
        [SerializeField]
        private float _animTime = 0.5f;
        [SerializeField]
        private Ease _animType = Ease.OutExpo;
        [SerializeField]
        [Range(0, 10)]
        private int _animQueueCount = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            gameObject.transform.localScale = Vector3.zero;
            var tween = transform.DOScale(Vector3.one, _animTime);
            tween.SetEase(_animType);
            tween.SetDelay(_animQueueCount * _animTime / 2);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DOTween.Kill(gameObject);
        }
    }
}
