using SHLibrary;
using SHLibrary.Logging;
using System.Collections.Generic;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.FormsCatalog
{
    public class DifferentPlatformFormsContainer : UnityBehaviourBase
    {
        [SerializeField]
        private FormUIType _targetPlatformUI;
        [SerializeField]
        private List<MarketPlatformUIConfig> _forms;
        [SerializeField]
        private CanvasScaler _canvasScaler;
        [SerializeField]
        private bool _autoDetectPlatformUI;
               
        private IPlatformUIConfig TargetContainer { get; set; }

        public FormUIType TargetUIType
        {
            get { return _targetPlatformUI; }
        }

        public TForm GetForm<TForm>() where TForm : WidgetFormViewBase
        {
            return TargetContainer.GetForm<TForm>();
        }

        public void DetectTargetForm()
        {
#if !UNITY_EDITOR && !UNITY_STANDALONE
    _autoDetectPlatformUI = true;
#endif
            var targetType = _autoDetectPlatformUI ? GetTargetUIType() : _targetPlatformUI;
            TargetContainer = _forms.Find(form => form.UIType == targetType);
            ApplyCanvasReferenceResolution();
            if (TargetContainer == null)
            {
                DevLogger.Error("Cannot find form container for " + targetType,
                    MarketLogType.UI);
            }

            _targetPlatformUI = targetType;
        }

        public void ApplyCustomPlatformUI(FormUIType platformUI)
        {
            _targetPlatformUI = platformUI;
            TargetContainer = _forms.Find(form => form.UIType == _targetPlatformUI);
            ApplyCanvasReferenceResolution();
            _autoDetectPlatformUI = false;
        }

        private FormUIType GetTargetUIType()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    return Screen.orientation == ScreenOrientation.Landscape
                    ? FormUIType.MobileLandscape
                    : FormUIType.MobilePortrait;
                default:
                    return FormUIType.Standalone;
            }
        }

        private void ApplyCanvasReferenceResolution()
        {
            _canvasScaler.referenceResolution = TargetContainer.CanvasReferenceResolution;
        }
    }
}