using DMarketSDK.Common.Forms.AnimationComponents;
using System.Collections.Generic;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Common.Forms
{
    public abstract class AnimMarketFormBase : WidgetFormViewBase
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
            return new List<IFormAnimationComponent>
            {
                new FadeAnimationComponent(new TweenAnimParameters(gameObject))
            };
        }
    }
}
