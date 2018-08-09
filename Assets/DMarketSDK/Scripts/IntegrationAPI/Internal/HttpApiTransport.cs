using SHLibrary.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DMarketSDK.IntegrationAPI.Settings;
using UnityEngine;
using UnityEngine.Networking;

namespace DMarketSDK.IntegrationAPI.Internal
{
    public class HttpApiTransport : MonoBehaviour, IApiTransport
    {
        private Uri _baseHost;
        private IBaseApiSettings _apiSettings;

        public void MakeCall(RequestMethod method, string path, string data, Dictionary<string, string> headers, ApiCallCallback callback)
        {
            if (_baseHost == null)
            {
                DevLogger.Error(string.Format("Need initialize {0} before MakeCall",
                    GetType()), MarketLogType.MarketApi);
                return;
            }
            var uri = new Uri(_baseHost, path);
            Log(string.Format("{0} request: {1} \n{2}", method.ToString().ToUpper(),
                GetShortUrl(uri.ToString()), data));
            switch (method)
            {
                case RequestMethod.Get:
                    StartCoroutine(MakeGet(uri, headers, callback, _apiSettings.UseDebug));
                    break;

                case RequestMethod.Post:
                    StartCoroutine(MakePost(uri, data, headers, callback, _apiSettings.UseDebug));
                    break;
                case RequestMethod.Put:
                    StartCoroutine(MakePut(uri, data, headers, callback, _apiSettings.UseDebug));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method", method, null);
            }
        }
         
        public void Initialize(IBaseApiSettings settings)
        {
            _baseHost = new Uri(settings.TargetEnvironmentUrl);
            _apiSettings = settings;
        }

        public void Dispose()
        {
            StopAllCoroutines();
            Destroy(this);
        }

        private static IEnumerator MakeGet(Uri uri, IDictionary<string, string> headers, ApiCallCallback callback, bool debug)
        {
            using (var www = UnityWebRequest.Get(uri.ToString()))
            {
                foreach (var h in headers)
                {
                    www.SetRequestHeader(h.Key, h.Value);
                }

#if UNITY_2017_1
                yield return www.Send();
#else
                yield return www.SendWebRequest();
#endif

                if (debug)
                {
                    var sb = new StringBuilder();

                    foreach (var h in headers)
                    {
                        sb.Append(string.Format("Key: {0}, value: {1}", h.Key, h.Value));
                    }
                    Log(string.Format("GET answer: {0} with code {1}\n{2}\nURI:{3}\nheaders:{4}", GetShortUrl(uri.ToString()),
                        www.responseCode, www.downloadHandler.text,
                        uri.ToString(), sb));
                }

                callback(www.downloadHandler.text, (int)www.responseCode);
            }
        }

        private static IEnumerator MakePost(Uri uri, string data, IDictionary<string, string> headers, ApiCallCallback callback, bool debug)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            
            using (var www = UnityWebRequest.Put(uri.ToString(), dataBytes))
            {
                www.method = "POST";
                headers.Add("Content-Type", "application/json");

                foreach (var h in headers)
                {
                    www.SetRequestHeader(h.Key, h.Value);
                }

                #if UNITY_2017_1
                yield return www.Send();
                #else
                yield return www.SendWebRequest();
                #endif

                if (debug)
                {
                    var headersBuilder = new StringBuilder();
                    
                    foreach (var h in headers)
                    {
                        headersBuilder.Append(string.Format("Key: {0}, Value: {1}", h.Key, h.Value));
                    }

                    string responseLog = string.Format("POST answer: {0} with code {1}\n{2}\n body:{3}" +
                                                       "\nURI:{4}\nheaders:{5}",
                        GetShortUrl(uri.ToString()),
                        www.responseCode, www.downloadHandler.text,
                        data, uri, headersBuilder);
                    string requestLog = string.Format("POST request: {0}, headers: {1}, request: {2}, code: {3}, response: {4}",
                        uri, headersBuilder, data, (int) www.responseCode, www.downloadHandler.text);

                    Log(responseLog + requestLog);
                }

                callback(www.downloadHandler.text, (int) www.responseCode);
            }
        }

        private static IEnumerator MakePut(Uri uri, string data, IDictionary<string, string> headers, ApiCallCallback callback, bool debug)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var www = UnityWebRequest.Put(uri.ToString(), dataBytes))
            {
                www.method = "PUT";
                headers.Add("Content-Type", "application/json");

                foreach (var h in headers)
                {
                    www.SetRequestHeader(h.Key, h.Value);
                }

#if UNITY_2017_1
                yield return www.Send();
#else
                yield return www.SendWebRequest();
#endif

                if (debug)
                {
                    var sb = new StringBuilder();

                    foreach (var h in headers)
                    {
                        sb.Append(string.Format("Key: {0}, value: {1}", h.Key, h.Value));
                    }

                    Debug.LogFormat("Url: {0}, headers: {1}, request: {2}, code: {3}, response: {4}", uri, sb, data, (int)www.responseCode, www.downloadHandler.text);

                    Log(string.Format("POST answer: {0} with code {1}\n{2}\n body:{3}" +
                        "\nURI:{4}\nheaders:{5}", GetShortUrl(uri.ToString()),
                      www.responseCode, www.downloadHandler.text,
                      data, uri.ToString(), sb));
                }

                callback(www.downloadHandler.text, (int)www.responseCode);
            }
        }

        //TODO refactoring after complete log system
        private static void Log(string message)
        {
            var title = string.Format("<color={0}>{1}</color>", "blue", MarketLogType.MarketApi);
            DevLogger.Log(message, title);
        }

        private static string GetShortUrl(string url)
        {
            var splitting = url.Split('/');
            return splitting[splitting.Length - 1];
        }
    }
}