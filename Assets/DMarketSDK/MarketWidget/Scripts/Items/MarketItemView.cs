using DMarketSDK.Market.Items.Components;
using SHLibrary.Logging;
using System;
using System.Collections.Generic;
using SHLibrary.ObserverView;
using DG.Tweening;
using UnityEngine;

namespace DMarketSDK.Market.Items
{
    public class MarketItemView : ObserverViewBase<MarketItemModel>
    {
        public Action<ItemActionType, MarketItemModel> Clicked;
        
        private List<ItemComponentBase> _components;

        public bool IsInitialize { get { return _components != null; } }

        public void Initialize()
        {
            if (!IsInitialize)
            {
                _components = new List<ItemComponentBase>();
                gameObject.GetComponentsInChildren(true, _components);
                _components.ForEach(component => component.ApplyItem(this));
            }
            else
            {
                DevLogger.Warning("Try initialize twice");
            }
        }

        public bool IsBlockForInput
        {
            get
            {
                if (!IsShowed || Model == null)
                {
                    return true;
                }

                foreach (var component in _components)
                {
                    if (component.IsNeedBlockInput)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsShowed
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        protected override void OnModelChanged()
        {
            if (IsInitialize)
            {
                _components.ForEach(component => component.ModelUpdate());
            }
        }
        
        public void ShowWithAnim(float time, float delay)
        {
            base.Show();
            DOTween.Kill(gameObject);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(Vector3.one, time).SetDelay(delay);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _components.ForEach(item =>item.RemoveItem());
            _components.Clear();
        }

        public override void Hide()
        {
            base.Hide();
            DOTween.Kill(gameObject);
        }
    }
}