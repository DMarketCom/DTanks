using UnityEngine;

namespace DMarketSDK.Market.Items.Components
{
    public class ItemSelectShowComponent : ItemSelectComponentBase
    {
        [SerializeField]
        private bool _isShow = true;

        public override void ModelUpdate()
        {
            base.ModelUpdate();
            var isSelected = Target.Model.IsSelected;
            SetState(isSelected);
        }

        protected override void SetState(bool value)
        {
            if (_isShow)
            {
                gameObject.SetActive(value);
            }
            else
            {
                gameObject.SetActive(!value);
            }
        }
    }
}
