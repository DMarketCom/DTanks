using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Common.Forms.AnimationComponents
{
    public class TweenAnimParameters
    {
        public readonly GameObject Target;
        public readonly float AnimTime = 0.7f;
        public readonly Ease AnimTypeShow = Ease.OutExpo;
        public readonly Ease AnimTypeHide = Ease.OutExpo;
        public readonly bool IsNeedPlayShow = true;
        public readonly bool IsNeedPlayHide = true;

        public TweenAnimParameters(GameObject target)
        {
            Target = target;
        }

        public TweenAnimParameters(GameObject target, float animTime, Ease animTypeShow = Ease.OutExpo, 
            Ease animTypeHide = Ease.InExpo,  bool isNeedPlayShow = true, bool isNeedPlayHide = true) : this(target)
        {
            AnimTypeShow = animTypeShow;
            AnimTypeHide = animTypeHide;
            AnimTime = animTime;
            IsNeedPlayShow = isNeedPlayShow;
            IsNeedPlayHide = isNeedPlayHide;
        }
    }
}
