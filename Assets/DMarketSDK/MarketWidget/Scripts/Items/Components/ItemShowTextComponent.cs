using UnityEngine;

namespace DMarketSDK.Market.Items.Components
{
    public class ItemShowTextComponent : ItemShowTextComponentBase
    {
        [SerializeField]
        private ShowType _type;
      
        protected override string GetShowingText()
        {
            var result = GetTargetString(_type, Target.Model);
            if (!string.IsNullOrEmpty(ShowFormat))
            {
                result = string.Format(ShowFormat, result);
            }
            return result;
        }
    }
}