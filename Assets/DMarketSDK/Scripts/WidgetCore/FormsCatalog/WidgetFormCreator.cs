using SHLibrary;
using System;
using System.Collections.Generic;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;
using UnityEngine;

namespace DMarketSDK.FormsCatalog
{
    public class WidgetFormCreator : UnityBehaviourBase
    {
        [SerializeField]
        private List<RectTransform> _layersParent;
        [SerializeField]
        private DifferentPlatformFormsContainer _formsCatalog;

        private DifferentPlatformFormsContainer FormsCatalog
        {
            get { return _formsCatalog; }
        }

        private List<WidgetFormViewBase> _forms = new List<WidgetFormViewBase>();

        private readonly Dictionary<MarketFormLayerType, RectTransform> _mapLayers
            = new Dictionary<MarketFormLayerType, RectTransform>();

        public FormUIType TargetUiType
        {
            get { return FormsCatalog.TargetUIType; }
        }

        public void Run()
        {
            AddLayers();
            FormsCatalog.DetectTargetForm();
        }

        public void Stop()
        {
            _mapLayers.Clear();
            foreach (var form in _forms)
            {
                form.Hide();
                Destroy(form.gameObject);
            }
            _forms.Clear();
        }

        private void AddLayers()
        {
            _mapLayers.Clear();
            var parentIndex = 0;
            foreach (var layerType in Enum.GetValues(typeof(MarketFormLayerType)))
            {
                _mapLayers.Add((MarketFormLayerType)layerType, _layersParent[parentIndex]);
                parentIndex++;
            }
        }

        private RectTransform GetLayerParent(MarketFormLayerType layerType)
        {
            return _mapLayers[layerType];
        }

        public TForm GetForm<TForm>() where TForm : WidgetFormViewBase
        {
            var resultForm = _forms.Find(form => form is TForm) as TForm;
            if (resultForm == null)
            {
                resultForm = GameObject.Instantiate(FormsCatalog.GetForm<TForm>());
                var layer = GetLayerParent(resultForm.Layer);
                resultForm.SetLayer(layer);
                _forms.Add(resultForm);
            }
            return resultForm;
        }
    }
}