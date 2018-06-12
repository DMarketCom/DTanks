using SHLibrary;
using SHLibrary.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DMarketSDK.Common.UI
{
    public class TabPanelContainer : UnityBehaviourBase
    {
        public const int NullIndex = -1;

        public event Action<int> Changed;

        [SerializeField]
        private RectTransform _content;

        private readonly List<TabViewBase> _tabs = new List<TabViewBase>();

        public bool Interactable
        {
            set
            {
                _tabs.ForEach(tab => tab.Interectible = value);
            }
        }

        public void BindWithChildTabs()
        {
            var currentTabs = new List<TabViewBase>();
            _content.GetComponentsInChildren<TabViewBase>(true, currentTabs);
            currentTabs.ForEach(AddTabButton);
            Interactable = true;
        }

        public void AddTabButton(TabViewBase tab)
        {
            _tabs.Add(tab);
            tab.Clicked += OnTabClicked;
            tab.SetState(false, false);
        }

        public void RemoveTabButton(TabViewBase tab)
        {
            _tabs.Remove(tab);
            if (tab != null)
            {
                tab.Clicked -= OnTabClicked;
                Destroy(tab.gameObject);
            }
        }

        public void SetActiveTab(int activeTabIndex = NullIndex, bool withAnim = true)
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                var alreadySelected = activeTabIndex == i && _tabs[i].IsSelected;
                if(alreadySelected)
                {
                    return;
                }
                _tabs[i].SetState(activeTabIndex == i, withAnim);
            }
        }

        protected override void OnDestroyObject()
        {
            base.OnDestroyObject();
            while (_tabs.Count > 0)
            {
                RemoveTabButton(_tabs[0]);
            }
        }

        private void OnTabClicked(TabViewBase tab)
        {
            _tabs.ForEach(item => item.SetState(item.Equals(tab), true));
            Changed.SafeRaise(_tabs.IndexOf(tab));
        }
    }
}