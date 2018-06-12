using SHLibrary.ObserverView;
using DMarketSDK.Common.Forms.AnimationComponents;
using System.Collections.Generic;

namespace DMarketSDK.Common.Forms
{
    public abstract class AnimFormBase : ViewBase
    {
        private List<IFormAnimationComponent> _animComponents;

        public override void Show()
        {
            GetAnimComponent().ForEach(anim => anim.Show());
        }

        public override void Hide()
        {
            GetAnimComponent().ForEach(anim => anim.Hide());
        }

        public void ApplyAnimComponent(List<IFormAnimationComponent> animComponents)
        {
            _animComponents = animComponents;
            _animComponents.ForEach(anim => anim.Initialize());
        }
        
        private List<IFormAnimationComponent> GetAnimComponent()
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
            var result = new List<IFormAnimationComponent>();
            result.Add(new FadeAnimationComponent(new TweenAnimParametrs(gameObject)));
            return result;
        }
    }
}