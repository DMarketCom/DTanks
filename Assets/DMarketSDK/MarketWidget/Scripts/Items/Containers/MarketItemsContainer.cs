using System;
using System.Collections.Generic;
using DMarketSDK.Market.Containers;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using SHLibrary.ObserverView;

namespace DMarketSDK.Market
{
    public abstract class MarketItemsContainer : ObserverViewBase<MarketContainerModel>
    {
        public event Action<ItemActionType, MarketItemModel> ItemClicked;
        public event Action<ShowingItemsInfo> UpdateShowingItemsInfo;

        protected readonly List<ContainerComponentBase> ContainerComponents = new List<ContainerComponentBase>();
        protected readonly List<ShowingContainerComponentBase> ShowingComponents = new List<ShowingContainerComponentBase>();

        public abstract int CurrentPage { get; }
        public abstract int ItemsPerPage { get; }
        public abstract bool IsInitialize { get; }

        protected override void Awake()
        {
            base.Awake();
            CacheShowingComponents();
            CacheContainerComponents();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO waiting Roma fixing doublicated showingInfo
            var currentShowingInfo = Model == null ? new ShowingItemsInfo() : Model.ShowingItemsInfo;
            ShowingComponents.ForEach(component => component.ResetShowingParams(currentShowingInfo));
            ShowingComponents.ForEach(component => component.Changed += OnComponentChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShowingComponents.ForEach(component => component.Changed -= OnComponentChanged);
        }

        public TComponent GetShowingComponent<TComponent>() where TComponent : ShowingContainerComponentBase
        {
            return ShowingComponents.Find(component => component is TComponent) as TComponent;
        }

        public override void ApplyModel(MarketContainerModel model)
        {
            base.ApplyModel(model);
            ContainerComponents.ForEach(c => c.ApplyModel(Model));
        }

        protected void UpdateAndNotifyShowingParams()
        {
            UpdateShowingParams();
            CallUpdateShowingItemsInfo(Model.ShowingItemsInfo);
        }

        protected void UpdateShowingParams()
        {
            Model.ShowingItemsInfo.Limit = ItemsPerPage;
            Model.ShowingItemsInfo.Offset = CurrentPage * ItemsPerPage;
            foreach (var showingComponent in ShowingComponents)
            {
                showingComponent.ModifyShowingParams(ref Model.ShowingItemsInfo);
            }
        }

        public abstract List<List<MarketItemView>> GetItemsGrid();

        protected virtual void OnComponentChanged()
        {
            UpdateAndNotifyShowingParams();
        }

        private void CacheShowingComponents()
        {
            gameObject.GetComponentsInChildren(true, ShowingComponents);
        }

        private void CacheContainerComponents()
        {
            gameObject.GetComponentsInChildren(true, ContainerComponents);
        }

        protected void CallItemClicked(ItemActionType actionType, MarketItemModel itemModel)
        {
            ItemClicked.SafeRaise(actionType, itemModel);
        }

        protected void CallUpdateShowingItemsInfo(ShowingItemsInfo itemsInfo)
        {
            UpdateShowingItemsInfo.SafeRaise(itemsInfo);
        }
    }
}