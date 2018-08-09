using UnityEngine;

namespace DMarketSDK.Market.Items.Components
{
    public class ItemShowArrayTextComponent : ItemShowTextComponentBase
    {
        [SerializeField]
        private ShowType[] _types;

        protected override string GetShowingText()
        {
            var lines = new object[_types.Length];
            for (var i = 0; i < _types.Length; i++)
            {
                lines[i] = GetTargetString(_types[i], Target.Model);
            }
            return string.Format(ShowFormat, lines);
        }
    }
}