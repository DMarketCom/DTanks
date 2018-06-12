using UnityEngine;
using UnityEngine.UI;
using SHLibrary.ObserverView;
using System.Collections.Generic;
using DMarketSDK.Widget.Forms;
using System;
using SHLibrary.Utils;

namespace DMarketSDK.Widget
{
    public class WidgetView : ObserverViewBase<WidgetModel>
    {
        public event Action ShowClicked;
        public event Action HideClicked;

        [SerializeField]
        private Button _btnShowWidget;
        [SerializeField]
        private Button _btnHideWidget;
        [SerializeField]
        private Button _btnBackground;
        [SerializeField]
        private GameObject _waitPanel = null;

        private List<WidgetFormBase> _forms = new List<WidgetFormBase>();

        public override void ApplyModel(WidgetModel model)
        {
            base.ApplyModel(model);
            gameObject.GetComponentsInChildren<WidgetFormBase>(true, _forms);
        }

        protected override void OnModelChanged()
        {   
        }

        public override void Show()
        {
            _btnShowWidget.gameObject.SetActive(false);
            _btnHideWidget.gameObject.SetActive(true);
            _btnBackground.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            _btnShowWidget.gameObject.SetActive(true);
            _btnHideWidget.gameObject.SetActive(false);
            _btnBackground.gameObject.SetActive(false);
        }

        public T GetForm<T>()
            where T : WidgetFormBase
        {
            return _forms.Find(form => form is T) as T;
        }

        public void ShowWaitPanel()
        {
            _waitPanel.SetActive(true);
        }

        public void HideWaitPanel()
        {
            _waitPanel.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnShowWidget.onClick.AddListener(OnShowWidgetClicked);
            _btnHideWidget.onClick.AddListener(OnHideWidgetClicked);
            _btnBackground.onClick.AddListener(OnHideWidgetClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnShowWidget.onClick.RemoveListener(OnShowWidgetClicked);
            _btnHideWidget.onClick.RemoveListener(OnHideWidgetClicked);
            _btnBackground.onClick.RemoveListener(OnHideWidgetClicked);

        }

        private void OnHideWidgetClicked()
        {
            HideClicked.SafeRaise();
        }

        private void OnShowWidgetClicked()
        {
            ShowClicked.SafeRaise();
        }

    }
}