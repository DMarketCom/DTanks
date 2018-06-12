using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Common.Forms.AnimationComponents
{
    public class TweenAnimParametrs
    {
        public readonly GameObject Target;
        public float AnimTime = 0.7f;
        public Ease AnimTypeShow = Ease.OutExpo;
        public Ease AnimTypeHide = Ease.OutExpo;
        public bool IsNeedPlayShow = true;
        public bool IsNeedPlayHide = true;

        public TweenAnimParametrs(GameObject target)
        {
            Target = target;
        }

        public TweenAnimParametrs(GameObject target, float animTime, Ease animTypeShow = Ease.OutExpo, 
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
