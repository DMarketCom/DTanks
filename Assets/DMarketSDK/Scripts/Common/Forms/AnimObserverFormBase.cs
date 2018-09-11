using SHLibrary.ObserverView;
using DMarketSDK.Common.Forms.AnimationComponents;
using System.Collections.Generic;

namespace DMarketSDK.Common.Forms
{
    public abstract class AnimObserverFormBase<T> : ObserverViewBase<T> where T : IObservable
    {
        protected List<IFormAnimationComponent> _animComponents;

        public override void Show()
        {
            GetAnimComponents().ForEach(anim => anim.Show());
        }

        public override void Hide()
        {
            GetAnimComponents().ForEach(anim => anim.Hide());
        }

        private List<IFormAnimationComponent> GetAnimComponents()
        {
            if (_animComponents == null)
            {
                _animComponents = CreateAnimComponents();
                _animComponents.ForEach(anim => anim.Initialize());
            }
            return _animComponents;
        }

        protected virtual List<IFormAnimationComponent> CreateAnimComponents()
        {
            return new List<IFormAnimationComponent>
            {
                new FadeAnimationComponent(new TweenAnimParameters(gameObject))
            };
        }
    }
}
