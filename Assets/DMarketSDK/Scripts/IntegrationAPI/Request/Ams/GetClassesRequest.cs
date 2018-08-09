using System;
using DMarketSDK.IntegrationAPI.Internal;

//List of classes
namespace DMarketSDK.IntegrationAPI.Request.Ams
{
    public class GetClassesRequest : BaseRequest<GetClassesRequest, GetClassesRequest.Response, GetClassesRequest.RequestParams>
    {
        private const string Path = "/ams/classes";
		

        public GetClassesRequest(string gameToken)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
            Params = new RequestParams
            {

			};
			WithGameToken(gameToken);
        }

        public class RequestParams
        {

		}

        public class Response
		{		
			public string id;
		}

        protected override string GetBasePath()
        {
			return Path;
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Get;
        }
    }
}