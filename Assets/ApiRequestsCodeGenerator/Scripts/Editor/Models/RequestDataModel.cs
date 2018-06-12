using System;
using System.Collections.Generic;

namespace CodeGenerator
{
    public class RequestDataModel
    {
        public string Folder;
        public string Namespace;
        public string ClassName;
        public string Url;
        public string Method;

        public List<ParameterModel> Params = new List<ParameterModel>();
        public List<ParameterModel> HeadParams = new List<ParameterModel>();
        public List<ParameterModel> BodyParams = new List<ParameterModel>();
        public List<ParameterModel> QueryParams = new List<ParameterModel>();
        public List<ParameterModel> PathParams = new List<ParameterModel>();
        public List<ParameterModel> Responces = new List<ParameterModel>();

        public Dictionary<string, string> SimpleReplace = new Dictionary<string, string>();

        private Func<string, DefinitionModel> GetShemeModelFunc;

        private Dictionary<string, string> ParamsMapping = new Dictionary<string, string> {
            { "x-game-token", ParameterName.gameToken.ToString() },
            { "x-basic-token", ParameterName.basicToken.ToString() },
            { "x-dmarket-token", ParameterName.dmarketToken.ToString() },
            { "x-basic-refresh-token", ParameterName.basicRefreshToken.ToString() },
            { "x-dmarket-refresh-token", ParameterName.dmarketRefreshToken.ToString() },
        };

        private Dictionary<string, string> TypesMapping = new Dictionary<string, string> {
             { "number", "float" },
             { "integer", "int" },
             { "array", "string[]" }
        };

        public RequestDataModel(string requestUrl, MethodModel model, string method, string basePath, string classNamePrefix = "", Func<string, DefinitionModel> getShemeModelFunc = null)
        {
            Url = requestUrl;
            Folder = Utils.GetFileName(requestUrl);
            Namespace = "." + Utils.GetNamespace(requestUrl); ;
            ClassName = classNamePrefix + Utils.GetClassName(Folder, requestUrl) + "Request";
            Folder = basePath + "/" + Folder;
            Method = method;

            GetShemeModelFunc = getShemeModelFunc;

            if (model.parameters != null)
            {
                ReplaceTypes(model.parameters);
                LoadParams(model.parameters);
            }

            SimpleReplace.Add("[ClassName]", ClassName);
            SimpleReplace.Add("[Url]", Url);
            SimpleReplace.Add("[Comment]", "//" + model.summary);
            SimpleReplace.Add("[Method]", Method);
            SimpleReplace.Add("[Namespace]", Namespace);


            Responces = ResponceShemeToParameters(GetResponce(model));
        }

        private void ReplaceTypes(ParameterModel[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = ReplaceType(parameters[i]);
            }
        }

        private ParameterModel ReplaceType(ParameterModel parameter)
        {
            if (!string.IsNullOrEmpty(parameter.type) && TypesMapping.ContainsKey(parameter.type))
            {
                TypesMapping.TryGetValue(parameter.type, out parameter.type);
            }

            return parameter;
        }

        private void LoadParams(ParameterModel[] parameters)
        {
            foreach (ParameterModel parameter in parameters)
            {
                //Mapping params name
                if (ParamsMapping.ContainsKey(parameter.name))
                {
                    ParamsMapping.TryGetValue(parameter.name, out parameter.name);
                }

                switch (parameter.In)
                {
                    case ParameterPosition.body: LoadBodyParams(parameter); break;
                    case ParameterPosition.header: HeadParams.Add(parameter); break;
                    case ParameterPosition.path: PathParams.Add(parameter); break;
                    case ParameterPosition.query: QueryParams.Add(parameter); break;
                }

                if (parameter.In != ParameterPosition.body)
                {
                    Params.Add(parameter);
                }
            }
        }

        private DefinitionModel GetResponce(MethodModel model, string code = "200")
        {
            foreach (KeyValuePair<string, ResponceModel> path in model.responses)
            {
                if (path.Key == code && !string.IsNullOrEmpty(path.Value.schema.refName))
                {
                    return GetShemeModelFunc.Invoke(path.Value.schema.refName);
                }
            }

            return null;
        }

        private void LoadBodyParams(ParameterModel parameter)
        {
            if (parameter.schema != null && !string.IsNullOrEmpty(parameter.schema.refName))
            {
                parameter.shemeModel = GetShemeModelFunc.Invoke(parameter.schema.refName);
                ShemeModelToParameters(parameter.shemeModel);
            }
        }

        private void ShemeModelToParameters(DefinitionModel model)
        {
            if (model == null || model.properties == null)
            {
                return;
            }

            foreach (KeyValuePair<string, PropertyModel> property in model.properties)
            {
                ParameterModel parameter = new ParameterModel
                {
                    In = ParameterPosition.body,
                    name = property.Key
                };
                if (!string.IsNullOrEmpty(property.Value.type))
                {
                    parameter.type = property.Value.type;
                }
                if (model.required.IndexOf(property.Key) != -1)
                {
                    parameter.required = true;
                }
                else
                {
                    parameter.required = false;
                }

                parameter = ReplaceType(parameter);

                BodyParams.Add(parameter);
                Params.Add(parameter);
            }
        }

        private List<ParameterModel> ResponceShemeToParameters(DefinitionModel model)
        {
            List<ParameterModel> responces = new List<ParameterModel>();
            if (model == null || model.properties == null)
            {
                return responces;
            }

            foreach (KeyValuePair<string, PropertyModel> property in model.properties)
            {
                ParameterModel parameter = new ParameterModel
                {
                    In = ParameterPosition.responce,
                    name = property.Key
                };
                if (!string.IsNullOrEmpty(property.Value.type))
                {
                    parameter.type = property.Value.type;
                }
                if (model.required.IndexOf(property.Key) != -1)
                {
                    parameter.required = true;
                }
                else
                {
                    parameter.required = false;
                }
                if (property.Value.items != null && !string.IsNullOrEmpty(property.Value.items.refName))
                {
                    DefinitionModel itemsModel = GetShemeModelFunc.Invoke(property.Value.items.refName);
                    parameter.items = ResponceShemeToParameters(itemsModel);
                }
                else if (!string.IsNullOrEmpty(property.Value.refName) && property.Value.refName == "#/definitions/representation.Money")
                {
                    parameter.type = "Price";
                }

                parameter = ReplaceType(parameter);

                responces.Add(parameter);
            }

            return responces;
        }
    }
}