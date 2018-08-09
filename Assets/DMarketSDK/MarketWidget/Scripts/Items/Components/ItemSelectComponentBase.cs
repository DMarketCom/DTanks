namespace DMarketSDK.Market.Items.Components
{
    public abstract class ItemSelectComponentBase : ItemComponentBase
    {
        protected virtual bool IsShowing
        {
            get { return gameObject.activeSelf; }
        }

        public override void ApplyItem(MarketItemView item)
        {
            base.ApplyItem(item);
            SetState(false);
        }

        public override void ModelUpdate()
        {
            base.ModelUpdate();
            var isSelected = Target.Model.IsSelected;
            if (IsShowing != isSelected)
            {
                SetState(isSelected);
            }
        }

        protected virtual void SetState(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
