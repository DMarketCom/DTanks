using System;
using SHLibrary.ObserverView;
using TankGame.Inventory.Items;
using TankGame.UI.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace TankGame.Inventory
{
    public sealed class InventoryView : ObserverViewBase<InventorySceneModel>
    {
        public event Action<InventoryItemClickType, long> ItemEvent;
        public event Action BackClicked;
        public event Action MarketWidgetClicked;

        [SerializeField]
        private MessageBoxForm _messageBoxForm;
        
        [SerializeField]
        public WaitingForm WaitingForm;

        [SerializeField]
        private InventoryItemsContainerView _itemsContainer;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private Button _marketButton;

        [SerializeField]
        private TextMeshProUGUI _marketButtonText;

        #region Notifications
        [SerializeField]
        private Transform _notificationPanel;

        public int CountNotificationForm = 5;

        [SerializeField]
        private SimpleMessageForm _notificationFormPrefab;

        private List<SimpleMessageForm> _notificationForms;
        #endregion

        public MessageBoxForm MessageBoxForm
        {
            get { return _messageBoxForm; }
        }

        protected override void Start()
        {
            CreateNotificationForms();
        }

        public override void Show()
        {
            base.Show();
            _itemsContainer.ApplyModel(Model);
        }

        public SimpleMessageForm CreateNotificationForm()
        {
            return _notificationForms.First(p => p.gameObject.activeSelf == false);
        }

        protected override void OnModelChanged()
        {
            _marketButtonText.text = Model.IsLoggedDMarket ? "DMarket" : "Sign in to DMarket";
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Subscribing(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Subscribing(false);
        }

        private void Subscribing(bool isSubscribe)
        {
            if (isSubscribe)
            {
                _itemsContainer.ItemClicked += OnItemClicked;
                _backButton.onClick.AddListener(OnBackClicked);
                _marketButton.onClick.AddListener(OnMarketWidgetClicked);
            }
            else
            {
                _itemsContainer.ItemClicked -= OnItemClicked;
                _backButton.onClick.RemoveListener(OnBackClicked);
                _marketButton.onClick.RemoveListener(OnMarketWidgetClicked);
            }
        }

        private void OnItemClicked(InventoryItemModel model, InventoryItemClickType actionType)
        {
            ItemEvent.SafeRaise(actionType, model.WorldId);
        }

        private void OnBackClicked()
        {
            BackClicked.SafeRaise();
        }

        private void OnMarketWidgetClicked()
        {
            MarketWidgetClicked.SafeRaise();
        }

        private void CreateNotificationForms()
        {
            _notificationForms = new List<SimpleMessageForm>();
            for (int i = 0; i < CountNotificationForm; i++)
            {
                _notificationForms.Add(Instantiate(_notificationFormPrefab, _notificationPanel));
            }
        }
    }
}