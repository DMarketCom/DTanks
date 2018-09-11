using DMarketSDK.Common.Forms.AnimationComponents;
using System.Collections.Generic;
using DMarketSDK.WidgetCore.Forms;

namespace DMarketSDK.Common.Forms
{
    public abstract class AnimMarketFormBase : WidgetFormViewBase
    {
        public void ApplyAnimComponent(List<IFormAnimationComponent> animComponents)
        {
            _animComponents = animComponents;
            _animComponents.ForEach(anim => anim.Initialize());
        }
    }
}
