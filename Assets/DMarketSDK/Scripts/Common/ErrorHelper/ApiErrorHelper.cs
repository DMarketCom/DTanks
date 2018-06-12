using DMarketSDK.IntegrationAPI;
using SHLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DMarketSDK.Common.ErrorHelper
{
    public class ApiErrorHelper : ScriptableObject, IApiErrorHelper
    {
        [Serializable]
        public class ErrorInfo
        {
            public ErrorCode ErrorCode;
            public string ErrorMessage;
        }

        [SerializeField]
        public List<ErrorInfo> ErrorsInfo;

        #region IApiErrorHelper implementation
        string IApiErrorHelper.GetErrorMessage(ErrorCode code)
        {
#if UNITY_EDITOR
            if (ErrorsInfo.Count == 0)
            {
                foreach (ErrorCode item in Enum.GetValues(typeof(ErrorCode)))
                {
                    var error = new ErrorInfo();
                    error.ErrorCode = item;
                    error.ErrorMessage = GetDefaultErrorMessage(error.ErrorCode);
                    ErrorsInfo.Add(error);
                    DevLogger.Log("Add default error info to catalog " + error.ErrorMessage);
                }
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
            var result = ErrorsInfo.Find(item => item.ErrorCode == code);
            return result != null ? result.ErrorMessage : GetDefaultErrorMessage(code);
        }
        #endregion

        private string GetDefaultErrorMessage(ErrorCode errorCode)
        {
            return Regex.Replace(errorCode.ToString(), "[A-Z]", " $0").Trim();
        }
    }
}