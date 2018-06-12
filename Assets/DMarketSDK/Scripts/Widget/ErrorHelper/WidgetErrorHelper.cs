using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DMarketSDK.Widget
{
    /// <summary>
    ///Dublicate from AirErrorHelper.cs (scriptable serialization not work with generic types) 
    /// </summary>
    public class WidgetErrorHelper : ScriptableObject, IWidgetErrorHelper
    {
        [Serializable]
        public class ErrorInfo
        {
            public WidgetErrorType ErrorCode;
            public string ErrorMessage;
        }

        [SerializeField]
        public List<ErrorInfo> ErrorsInfo;

        #region IWidgetErrorHelper implementation
        string IWidgetErrorHelper.GetErrorMessage(WidgetErrorType code)
        {
            var result = ErrorsInfo.Find(item => item.ErrorCode == code);
            return result != null ? result.ErrorMessage : GetDefaultErrorMessage(code);
        }
        #endregion
        
        private string GetDefaultErrorMessage(WidgetErrorType errorCode)
        {
            return Regex.Replace(errorCode.ToString(), "[A-Z]", " $0").Trim();
        }
    }
}