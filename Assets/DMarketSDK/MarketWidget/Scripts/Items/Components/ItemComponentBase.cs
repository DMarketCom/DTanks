using SHLibrary;

namespace DMarketSDK.Market.Items.Components
{
    public abstract class ItemComponentBase : UnityBehaviourBase
    {
        protected MarketItemView Target { get; private set; }

        public virtual bool IsNeedBlockInput
        {
            get { return false; }
        }

        public virtual void ApplyItem(MarketItemView item)
        {
            Target = item;
            if (Target.Model != null)
            {
                ModelUpdate();
            }
        }

        public virtual void RemoveItem()
        {
            Target = null;
        }

        public virtual void ModelUpdate()
        { }
    }
}