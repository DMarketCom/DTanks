using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMarketSDK.IntegrationAPI.Internal
{
    public interface IRequest
    {
        string GetBodyString();
        string GetPath();
        RequestMethod GetMethod();
        Dictionary<string, string> GetHeaders();
    }

    public delegate void ResultCallback<in TResult, in TRequest>(TResult result, TRequest request);

    public abstract class BaseRequest<TRequest, TResponse, TRequestParams> : IRequest
        where TResponse : new()
        where TRequest : IRequest
    {
        private const string GameTokenName = "x-game-token";
        private const string BasicTokenName = "x-basic-token";
        private const string DMarketTokenName = "x-dmarket-token";
        private const string BasicRefreshTokenName = "x-basic-refresh-token";
        private const string DMarketRefreshTokenName = "x-dmarket-refresh-token";

        private ResultCallback<TResponse, TRequestParams> _resultCallback;
        private ErrorCallback _errorCallback;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        protected TRequestParams Params;

        public abstract RequestMethod GetMethod();

        protected abstract string GetBasePath();

        protected virtual Dictionary<string, object> GetBody()
        {
            return new Dictionary<string, object>();
        }

        protected virtual Dictionary<string, object> GetQuery()
        {
            return new Dictionary<string, object>();
        }

        public virtual string GetBodyString()
        {
            var body = GetBody();
            body = ClearDefaultValues(body);
            if (body.Count == 0)
            {
                return GetMethod() == RequestMethod.Get ? null : " ";
            }
            return JsonConvert.SerializeObject(body, Formatting.Indented);
        }

        public string GetPath()
        {
            var result = new StringBuilder();
            result.Append(GetBasePath());
            var query = GetQuery();
            query = ClearDefaultValues(query);
            if (query.Count == 0)
            {
                return result.ToString();
            }
            result.Append("?");
            foreach (var key in query.Keys)
            {
                result.Append(string.Format("{0}={1}&", key, query[key]));
            }
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public void Execute(IApiTransport transport)
        {
            if (transport == null) throw new ArgumentNullException("transport");

            ApiCallCallback callback = delegate (string response, int code)
            {
                TResponse result;
                Error error;

                ResultContainer<TResponse>.HandleResults(response, code, out result, out error);

                if (error != null && _errorCallback != null)
                {
                    _errorCallback(error);
                    return;
                }

                if (_resultCallback != null)
                {
                    _resultCallback(result, Params);
                }
            };

            transport.MakeCall(GetMethod(), GetPath(), GetBodyString(), GetHeaders(), callback);
        }

        private Dictionary<string, object> ClearDefaultValues(Dictionary<string, object> query)
        {
            var result = new Dictionary<string, object>();
            foreach (var key in query.Keys)
            {
                var item = query[key];
                bool isNeedAddItem = item != null;
                if (isNeedAddItem && (item is int))
                {
                    isNeedAddItem = (int)item != 0;
                }
                else if (isNeedAddItem && (item is long))
                {
                    isNeedAddItem = (long)item != 0;
                }
                else if (isNeedAddItem && item is string)
                {
                    isNeedAddItem = (string)item != string.Empty;
                }
                else if (isNeedAddItem && (item is float || item is double))
                {
                    isNeedAddItem = (float) item >= 0.001f;
                }

                if (isNeedAddItem)
                {
                    result.Add(key, item);
                }
            }
            return result;
        }

        public Dictionary<string, string> GetHeaders()
        {
            return _headers;
        }

        public BaseRequest<TRequest, TResponse, TRequestParams> WithCallback(ResultCallback<TResponse, TRequestParams> callback)
        {
            _resultCallback = callback;
            return this;
        }

        public BaseRequest<TRequest, TResponse, TRequestParams> WithErrorCallback(ErrorCallback callback)
        {
            _errorCallback = callback;
            return this;
        }

        protected BaseRequest<TRequest, TResponse, TRequestParams> WithDMarketToken(string token)
        {
            return WithHeader(DMarketTokenName, token);
        }
        
        protected BaseRequest<TRequest, TResponse, TRequestParams> WithBasicToken(string token)
        {
            return WithHeader(BasicTokenName, token);
        }
        
        protected BaseRequest<TRequest, TResponse, TRequestParams> WithGameToken(string token)
        {
            return WithHeader(GameTokenName, token);
        }

        protected BaseRequest<TRequest, TResponse, TRequestParams> WithBasicRefreshToken(string token)
        {
            return WithHeader(BasicRefreshTokenName, token);
        }

        protected BaseRequest<TRequest, TResponse, TRequestParams> WithDMarketRefreshToken(string token)
        {
            return WithHeader(DMarketRefreshTokenName, token);
        }

        private BaseRequest<TRequest, TResponse, TRequestParams> WithHeader(string name, string value)
        {
            _headers.Add(name, value);
            return this;
        }
    }
}