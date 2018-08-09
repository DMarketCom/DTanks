using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Common.Forms.AnimationComponents
{
    public abstract class TweenAnimationComponentBase : IFormAnimationComponent
    {
        #region IFormAnimationComponent implementation
        void IFormAnimationComponent.Initialize()
        {
            TransformAnchor = Target.GetComponent<RectTransform>();
            IsShowing = false;
            Target.SetActive(IsShowing);
            OnInitialize();
        }

        void IFormAnimationComponent.Show()
        {
            TransformAnchor.SetAsLastSibling();
            ChangeState(true);
            OnShowStart();
        }

        void IFormAnimationComponent.Hide()
        {
            ChangeState(false);
            OnHideStart();
        }
        #endregion

        private readonly TweenAnimParameters _animInfo;
        private Tweener _currentAnim;

        protected GameObject Target { get { return _animInfo.Target; } }
        protected bool IsShowing { get; private set; }
        protected RectTransform TransformAnchor { get; private set; }
        protected float AnimTime { get { return _animInfo.AnimTime; } }

        public TweenAnimationComponentBase(TweenAnimParameters tweenParameters)
        {
            _animInfo = tweenParameters;
        }

        protected abstract Tweener PlayBaseAnim(bool forShow);

        protected virtual void OnInitialize()
        { }

        protected virtual void OnShowStart()
        { }

        protected virtual void OnHideStart()
        { }

        private void ChangeState(bool show)
        {
            if (IsShowing != show)
            {
                IsShowing = show;
                if (_currentAnim != null)
                {
                    _currentAnim.Kill(false);
                }
                if ((IsShowing && _animInfo.IsNeedPlayShow)
                    || (!IsShowing && _animInfo.IsNeedPlayHide))
                {
                    Target.SetActive(true);
                    _currentAnim = PlayBaseAnim(IsShowing);
                    _currentAnim.OnComplete(OnAnimComplete);
                    var animType = IsShowing ? _animInfo.AnimTypeShow : _animInfo.AnimTypeHide;
                    _currentAnim.SetEase(animType);
                }
            }
        }

        private void OnAnimComplete()
        {
            Target.SetActive(IsShowing);
            _currentAnim = null;
        }
    }
}