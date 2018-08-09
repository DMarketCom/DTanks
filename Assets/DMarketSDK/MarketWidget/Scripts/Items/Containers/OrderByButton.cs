using DMarketSDK.Market.Domain;
using SHLibrary;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market
{
    public class OrderByButton : UnityBehaviourBase
    {
        public event Action<OrderByButton, OrderDirectionType> Clicked;

        [SerializeField]
        private Button _orderButton;
        [SerializeField]
        private TextMeshProUGUI _titleText;
        [SerializeField]
        private Image _orderByNone;
        [SerializeField]
        private Image _orderByMax;
        [SerializeField]
        private Image _orderByMin;

        private OrderDirectionType _orderDir;

        public bool Interactable
        {
            get { return _orderButton.interactable; }
            set { _orderButton.interactable = value; }
        }

        public string Title
        {
            get { return _titleText.text; }
            set { _titleText.text = value; }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _orderButton.onClick.AddListener(OnBtnClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _orderButton.onClick.RemoveListener(OnBtnClick);
        }

        public void SetState(OrderDirectionType orderDirection)
        {
            _orderDir = orderDirection;
            _orderByMax.enabled = _orderDir == OrderDirectionType.Asc;
            _orderByMin.enabled = _orderDir == OrderDirectionType.Desc;
            _orderByNone.enabled = _orderDir == OrderDirectionType.None;
        }

        private void OnBtnClick()
        {
            switch (_orderDir)
            {
                case OrderDirectionType.None:
                case OrderDirectionType.Desc:
                    SetState(OrderDirectionType.Asc);
                    break;
                case OrderDirectionType.Asc:
                    SetState(OrderDirectionType.Desc);
                    break;
            }
            Clicked.SafeRaise(this, _orderDir);
        }
    }
}