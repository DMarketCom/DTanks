using System;
using System.Collections.Generic;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;
using UnityEngine;

namespace DMarketSDK.FormsCatalog
{
    public sealed class MarketPlatformUIConfig : ScriptableObject, IPlatformUIConfig
    {
        [SerializeField]
        private Vector2 _canvasReferenceResolution;

        [SerializeField]
        private FormUIType _UIType;
        [SerializeField]
        private List<WidgetFormViewBase> _forms;

        public FormUIType UIType { get { return _UIType; } }

        #region IPlatformUIConfig implementation

        Vector2 IPlatformUIConfig.CanvasReferenceResolution
        {
            get { return _canvasReferenceResolution; }
        }

        TForm IPlatformUIConfig.GetForm<TForm>()
        {
            var result = _forms.Find(form => form is TForm) as TForm;
            if (result == null)
            {
                throw new Exception(string.Format("Need add {0} for to {1}",
                    typeof(TForm), name));
            }
            return result;
        }

        #endregion
    }
}