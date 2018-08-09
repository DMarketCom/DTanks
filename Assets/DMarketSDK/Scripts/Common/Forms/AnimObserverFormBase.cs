using SHLibrary.ObserverView;
using DMarketSDK.Common.Forms.AnimationComponents;

namespace DMarketSDK.Common.Forms
{
    public abstract class AnimObserverFormBase<T> : ObserverViewBase<T> where T : IObservable
    {
        private IFormAnimationComponent _anim;

        public override void Show()
        {
            GetAnimComponent().Show();
        }

        public override void Hide()
        {
            GetAnimComponent().Hide();
        }

        private IFormAnimationComponent GetAnimComponent()
        {
            if (_anim == null)
            {
                _anim = CreateAnimComponent();
                _anim.Initialize();
            }
            return _anim;
        }

        protected virtual IFormAnimationComponent CreateAnimComponent()
        {
            return new FadeAnimationComponent(new TweenAnimParameters(gameObject));
        }
    }
}
