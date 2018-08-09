using DMarketSDK.IntegrationAPI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SHLibrary.Logging;
using UnityEngine;

namespace DMarketSDK.Common.ErrorHelper
{
    public enum LanguageType
    {
        EN_US,
        EN_UK,
        RU_RU
    }

    public enum LanguageFileVersion
    {
        Offline,
        Online
    }

    public class ApiErrorHelper : MonoBehaviour, IApiErrorHelper
    {
        public string Url;
        public TextAsset ErrorListFile;
        public LanguageType DefaultLanguage;
        public LanguageFileVersion Version;

        private List<ErrorInfo> ErrorsInfo;
        private string[][] _grid;

        private class ErrorInfo
        {
            public ErrorCode ErrorCode;
            public string ErrorMessage;
        }

        private void Start()
        {
            LoadDataSheet();
        }

        #region IApiErrorHelper implementation
        public void ChangeLanguage(LanguageType lang)
        {
            ErrorsInfo = new List<ErrorInfo>();
            if (_grid.Length > 0 && _grid[0].Length > 0)
            {
                int CurrentLanguageColumn = 1;
                for (int column = 0; column < _grid[0].Length; column++)
                {
                    if (_grid[0][column] == lang.ToString())
                    {
                        CurrentLanguageColumn = column;
                        break;
                    }
                }

                for (int row = 1; row < _grid.Length; row++)
                {
                    var column = 0;
                    var errorCode = ErrorCode.Unknown;
                    try
                    {
                        errorCode = (ErrorCode) Enum.Parse(typeof(ErrorCode), _grid[row][column]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        DevLogger.Error(string.Format("The are no error in row {0} column {1}",
                            row, column));
                    }
                    catch (Exception e)
                    {
                        DevLogger.Error(string.Format("Cannot parse error code {0}. Exception: {1}",
                            _grid[row][column], e.Message));
                    }
                    
                    var error = new ErrorInfo
                    {
                        ErrorCode = errorCode,
                        ErrorMessage = _grid[row][CurrentLanguageColumn]
                    };
                    ErrorsInfo.Add(error);
                }
            }
            else
            {
                throw new ArgumentNullException("Wrong format of spreadsheet in ApiErrorHelper");
            }
        }

        string IApiErrorHelper.GetErrorMessage(ErrorCode code)
        {
            var result = ErrorsInfo.Find(item => item.ErrorCode == code);
            return result != null ? result.ErrorMessage : GetDefaultErrorMessage(code);
        }
        #endregion


        private void LoadDataSheet()
        {
            if (Version == LanguageFileVersion.Online)
            {
                StartCoroutine(ApiErrorHelperLoader.GetRequest(Url, LoadDataSheetCallback, ErrorCallback));
            }
            else
            {
                LoadDataSheetCallback(ErrorListFile.text);
            } 
        }

        private void LoadDataSheetCallback(string data)
        {
            _grid = CsvParser.Parse(ErrorListFile.text);
            ChangeLanguage(DefaultLanguage);
        }

        private void ErrorCallback(string data)
        {
            LoadDataSheetCallback(ErrorListFile.text);
        }

        private string GetDefaultErrorMessage(ErrorCode errorCode)
        {
            return Regex.Replace(errorCode.ToString(), "[A-Z]", " $0").Trim();
        }
    }
}