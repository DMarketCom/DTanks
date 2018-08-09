using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Items.Components
{
    [RequireComponent(typeof(Button))]
    public class ItemButtonComponent : ItemComponentBase
    {
        private Button _btn;

        [SerializeField]
        private ItemActionType _actionType;

        public override void ApplyItem(MarketItemView item)
        {
            base.ApplyItem(item);
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClicked);
        }

        public override void RemoveItem()
        {
            base.RemoveItem();
            _btn.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            if (!Target.Model.IsLockFromServer)
            {
                Target.Clicked.SafeRaise(_actionType, Target.Model);
            }
        }
    }
}