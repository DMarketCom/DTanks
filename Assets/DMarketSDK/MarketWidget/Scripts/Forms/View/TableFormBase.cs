using System;
using System.Collections.Generic;
using SHLibrary.AdditionalTypes;
using DMarketSDK.Common.Navigation;
using DMarketSDK.Market.Items;
using DMarketSDK.WidgetCore.Forms;
using UnityEngine;

namespace DMarketSDK.Market.Forms
{
    public abstract class TableFormBase<T> : WidgetFormViewBase<T>, IFormWithItems where T : ItemsFormModel
    {
        #region IFormWithItems implementation

        public event Action<ItemActionType, MarketItemModel> ItemClicked;

        #endregion

        [SerializeField]
        private MarketItemsContainer _container;

        public abstract ItemModelType ItemType { get; }

        public MarketItemsContainer Container { get { return _container; } }

        public int ItemPerPage { get { return Container.ItemsPerPage; } }

        public int CurrentPage { get { return Container.CurrentPage; } }

        protected override void OnEnable()
        {
            base.OnEnable();

            Container.ItemClicked += OnItemClicked;
            NavigationInputBase.Clicked += OnNavigationClicked;
        }

        private void OnNavigationClicked(NavigationType navigationType)
        {
            var grid = _container.GetItemsGrid();
            var currentItemPos = DetectCurrentItemPos(grid);
            var targetItem = NavigationUtil.GetTarget(grid,
                currentItemPos, navigationType);
        
            if (targetItem != null && !targetItem.IsBlockForInput)
            {
                FormModel.SelectedItem = targetItem.Model;
                FormModel.SetChanges();
            }
        }

        private IntVector2 DetectCurrentItemPos(List<List<MarketItemView>> grid)
        {
            var currentItemPos = new IntVector2();
            for (var z = 0; z < grid.Count; z++)
            {
                for (var x = 0; x < grid[z].Count; x++)
                {
                    if (grid[z][x].Model.IsSelected)
                    {
                        currentItemPos.Z = z;
                        currentItemPos.X = x;
                    }
                }
            }

            return currentItemPos;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Container.ItemClicked -= OnItemClicked;
            NavigationInputBase.Clicked -= OnNavigationClicked;
        }

        protected void OnItemClicked(ItemActionType actionType, MarketItemModel model)
        {
            ItemClicked.SafeRaise(actionType, model);
        }
    }
}