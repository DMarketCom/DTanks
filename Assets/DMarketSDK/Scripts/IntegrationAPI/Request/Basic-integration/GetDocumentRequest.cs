using System;
using DMarketSDK.IntegrationAPI.Internal;

//Document by it's type
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class GetDocumentRequest : BaseRequest<GetDocumentRequest, GetDocumentRequest.Response, GetDocumentRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/document/{type}";
		private string _type = default(string);

        public GetDocumentRequest(string type, string basicToken)
        {
			if (string.IsNullOrEmpty(basicToken)) throw new ArgumentNullException("basicToken");
			this._type = type;
            Params = new RequestParams
            {

			};
			WithBasicToken(basicToken);
        }

        public class RequestParams
        {

		}

        public class Response
		{		
			public string body;
		}

        protected override string GetBasePath()
        {

			return Path.Replace("{type}", _type);
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Get;
        }
    }
}