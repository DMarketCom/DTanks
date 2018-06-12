using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeGenerator
{
    enum ParameterName
    {
        basicToken,
        gameToken,
        dmarketToken,
        basicRefreshToken,
        dmarketRefreshToken
    }

    public class ParamsReplacer
    {
        RequestDataModel _request;
        string _classText;

        public ParamsReplacer(string template, RequestDataModel request)
        {
            _classText = template;
            _request = request;
        }

        public string Process()
        {
            ReplaceSimple();
            InputParameters();
            VerifyParameters();
            WithParameters();
            PatchParameters();
            BodyParameters();
            QueryParameters();
            ResponseParams();

            return _classText;
        }

        private void ReplaceSimple()
        {
            foreach (KeyValuePair<string, string> Param in _request.SimpleReplace)
            {
                _classText = _classText.Replace(Param.Key, Param.Value);
            }
        }

        #region [InputParameters]
        private void InputParameters()
        {
            _request.Params = _request.Params.OrderBy(e => e.required ? false : true).ToList();

            List<string> InputParameters = new List<string>();
            foreach (ParameterModel Parametr in _request.Params)
            {
                string InputParameter = Parametr.type + " " + Parametr.name;
                if (!Parametr.required)
                {
                    InputParameter += " = default(" + Parametr.type + ")";
                }
                InputParameters.Add(InputParameter);
            }

            _classText = _classText.Replace("[InputParameters]", string.Join(", ", InputParameters.ToArray()));
        }
        #endregion

        #region [VerifyParameters]
        private void VerifyParameters()
        {
            List<ParameterModel> ParamsList = new List<ParameterModel>();
            foreach (ParameterModel Parametr in _request.HeadParams)
            {
                if (Parametr.required && Parametr.type == "string")
                {
                    ParamsList.Add(Parametr);
                }
            }
            ByLineParamenters(ParamsList, "[VerifyParameters]", GetVerifyParameter);
        }

        private string GetVerifyParameter(string parameterName)
        {
            return "\t\t\tif (string.IsNullOrEmpty(" + parameterName + ")) throw new ArgumentNullException(\"" + parameterName + "\");";
        }
        #endregion

        #region [WithParameters]
        private void WithParameters()
        {
            List<ParameterModel> ParamsList = new List<ParameterModel>();
            foreach (ParameterModel Parametr in _request.HeadParams)
            {
                if (Parametr.type == "string")
                {
                    ParamsList.Add(Parametr);
                }
            }
            ByLineParamenters(ParamsList, "[WithParameters]", GetWithParameter);
        }

        private string GetWithParameter(string parameter)
        {
            ParameterName parameterName = (ParameterName)Enum.Parse(typeof(ParameterName), parameter);
            switch (parameterName)
            {
                case ParameterName.basicToken: return "\t\t\tWithBasicToken(" + parameter + ");";
                case ParameterName.gameToken: return "\t\t\tWithGameToken(" + parameter + ");";
                case ParameterName.dmarketToken: return "\t\t\tWithDMarketToken(" + parameter + ");";
                case ParameterName.basicRefreshToken: return "\t\t\tWithBasicRefreshToken(" + parameter + ");";
                case ParameterName.dmarketRefreshToken: return "\t\t\tWithDMarketRefreshToken(" + parameter + ");";
                default: return string.Empty;
            }
        }
        #endregion

        #region PatchParameters - [ClassParamsInit], [ClassParams], [PathReplaceParams]
        private void PatchParameters()
        {
            List<string> ClassParamsInit = new List<string>();
            List<string> ClassParams = new List<string>();
            string PathReplaceParams = string.Empty;
            foreach (ParameterModel Parametr in _request.PathParams)
            {
                ClassParamsInit.Add("private " + Parametr.type + " _" + Parametr.name + " = default(" + Parametr.type + ");");
                ClassParams.Add("\t\t\tthis._" + Parametr.name +" = "+ Parametr.name + ";");
                PathReplaceParams += ".Replace(\"{" +Parametr.name+ "}\", _" + Parametr.name + ")";
            }

            _classText = _classText.Replace("[ClassParamsInit]", string.Join("\n\r", ClassParamsInit.ToArray()));
            _classText = _classText.Replace("[ClassParams]", string.Join("\n\r", ClassParams.ToArray()));

            if (_request.PathParams.Count > 0)
            {
                _classText = _classText.Replace("[PathReplaceParams]", "\n\t\t\treturn Path" + PathReplaceParams + ";");
            }
            else
            {
                _classText = _classText.Replace("[PathReplaceParams]", "\t\t\treturn Path;");
            }
        }
        #endregion

        #region BodyParameters - [RequestParams], [SetRequestParams], [GetBodyRequestParams]
        private void BodyParameters()
        {
            List<string> RequestParams = new List<string>();
            List<string> SetRequestParams = new List<string>();
            List<string> GetBodyRequestParams = new List<string>();
            foreach (ParameterModel Parametr in _request.BodyParams)
            {
                RequestParams.Add("\t\t\tpublic " + Parametr.type + " " + Parametr.name + ";");
                SetRequestParams.Add("\t\t\t\t" + Parametr.name + " = " + Parametr.name);
                GetBodyRequestParams.Add("\t\t\t{\"" + Parametr.name + "\", Params."+ Parametr.name + "}");
            }

            _classText = _classText.Replace("[RequestParams]", string.Join("\n", RequestParams.ToArray()));
            _classText = _classText.Replace("[SetRequestParams]", string.Join(",\n", SetRequestParams.ToArray()));

            if (_request.BodyParams.Count > 0)
            {
                _classText = _classText.Replace("[GetBodyRequestParams]", "\n\t\tprotected override Dictionary<string, object> GetBody()\n" +
                    "\t\t{\n" +
                    "\t\t\treturn new Dictionary<string, object>(){\n" +
                        "\t" + string.Join(",\n", GetBodyRequestParams.ToArray()) +
                    "\n\t\t\t};\n\t\t}");
            }
            else
            {
                _classText = _classText.Replace("[GetBodyRequestParams]", string.Empty);
            }
        }
        #endregion

        #region QueryParameters - [QueryParameters], [SetQueryParams], [GetQueryParams]
        private void QueryParameters()
        {
            List<string> RequestParams = new List<string>();
            List<string> SetRequestParams = new List<string>();
            List<string> GetQueryParams = new List<string>();
            foreach (ParameterModel Parametr in _request.QueryParams)
            {
                RequestParams.Add("\t\t\tpublic " + Parametr.type + " " + Parametr.name + ";");
                SetRequestParams.Add("\t\t\t\t" + Parametr.name + " = " + Parametr.name);
                GetQueryParams.Add("\t\t\t{\"" + Parametr.name + "\", Params." + Parametr.name + "}");
            }

            _classText = _classText.Replace("[QueryParameters]", string.Join("\n", RequestParams.ToArray()));
            _classText = _classText.Replace("[SetQueryParams]", string.Join(",\n", SetRequestParams.ToArray()));

            if (_request.QueryParams.Count > 0)
            {
                _classText = _classText.Replace("[GetQueryParams]", "\n\t\tprotected override Dictionary<string, object> GetQuery()\n" +
                    "\t\t{\n" +
                    "\t\t\treturn new Dictionary<string, object>(){\n" +
                        "\t" + string.Join(",\n", GetQueryParams.ToArray()) +
                    "\t\t\n};\n" +
                 "\t\t}");
            }
            else
            {
                _classText = _classText.Replace("[GetQueryParams]", string.Empty);
            }
        }
        #endregion

        #region [ResponseParams]
        private void ResponseParams()
        {
            List<string> ResponseParams = GetResponceParams(_request.Responces);

            _classText = _classText.Replace("[ResponseParams]", string.Join("\n", ResponseParams.ToArray()));
        }

        private List<string> GetResponceParams(List<ParameterModel> Responces)
        {
            List<string> Params = new List<string>();

            foreach (ParameterModel Parametr in Responces)
            {
                if (Parametr.items != null)
                {
                    List<string> SubParams = GetResponceParams(Parametr.items);
                    Debug.Log(Parametr.items);
                    Params.Add("\t\t\tpublic class MarketAsset\n" +
                    "\t\t\t{\n\t" +
                    string.Join("\n\t", SubParams.ToArray()) +
                    "\n\t\t\t}" +
                    "\n\t\t\tpublic List<MarketAsset> Items;\n");
                }
                else
                {
                    Params.Add("\t\t\tpublic " + Parametr.type + " " + Parametr.name + ";");
                }
            }

            return Params;
        }
        #endregion

        private void ByLineParamenters(List<ParameterModel> paramsList, string tag, Func<string, string> getParamFunc, string separator = "\n")
        {
            List<string> Lines = new List<string>();
            foreach (ParameterModel Parametr in paramsList)
            {
                string Line = getParamFunc.Invoke(Parametr.name);
                if (!string.IsNullOrEmpty(Line))
                {
                    Lines.Add(Line);
                }
            }

            _classText = _classText.Replace(tag, string.Join(separator, Lines.ToArray()));
        }
    }
}
