using DMarketSDK.Market.Items;
using UnityEngine;

namespace DMarketSDK.Market.Forms
{
	public class BuyClassItemsForm : TableFormBase<ItemsFormModel>
    {
        [SerializeField]
        private MarketItemView _marketItemView;

        public override ItemModelType ItemType
		{
			get
			{
				return ItemModelType.BuyItem;
			}
		}

        private void Awake()
        {
            _marketItemView.Initialize();
            _marketItemView.Show();
        }

        protected override void OnModelChanged()
        {
            base.OnModelChanged();
            _marketItemView.ApplyModel(FormModel.SelectedItem);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _marketItemView.Clicked += OnItemClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _marketItemView.Clicked -= OnItemClicked;
        }
    }
}
