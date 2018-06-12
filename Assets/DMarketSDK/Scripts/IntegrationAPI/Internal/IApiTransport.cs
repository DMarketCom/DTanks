using System.Collections.Generic;

namespace DMarketSDK.IntegrationAPI.Internal
{
    public enum RequestMethod
    {
        Get = 1,
        Post = 2,
        Put = 3
    }
    
    public delegate void ApiCallCallback(string response, int code);
    
    public interface IApiTransport
    {
        void MakeCall(RequestMethod method, string path, string data, Dictionary<string, string> headers, ApiCallCallback callback);
    }
}