using SHLibrary.Logging;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace DMarketSDK.Common.ErrorHelper
{
    public class ApiErrorHelperLoader
    {
        public static IEnumerator GetRequest(string url, Action<string> callback, Action<string> errorCallback)
        {
            using (var www = UnityWebRequest.Get(url))
            {
                www.SetRequestHeader("Content-Type", "text/csv");
#if UNITY_2017_1
                yield return www.Send();
#else
                yield return www.SendWebRequest();
#endif
                if (www.responseCode == 200)
                {
                    callback(www.downloadHandler.text);
                }
                else
                {
                    DevLogger.Log(string.Format("ApiErrorHelper can't load spreadsheet from {0}\nError: {1}", url, www.error));
                    errorCallback(www.error);
                }
            }
        }
    }
}
