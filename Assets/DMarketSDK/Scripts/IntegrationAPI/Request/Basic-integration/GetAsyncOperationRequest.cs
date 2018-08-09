using System;
using DMarketSDK.IntegrationAPI.Internal;

//Get operation status
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class GetAsyncOperationRequest : BaseRequest<GetAsyncOperationRequest, GetAsyncOperationRequest.Response, GetAsyncOperationRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/async/operation/{operationId}";
		private string _operationId = default(string);

        public GetAsyncOperationRequest(string operationId, string gameToken, string dmarketToken)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
			this._operationId = operationId;
            Params = new RequestParams
            {

			};
			WithGameToken(gameToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {

		}

        public class Response
		{		
			public string assetId;
			public string classId;
			public string operation;
			public string operationId;
			public string status;
			public TransferErrorModel transferError;
		}

        protected override string GetBasePath()
        {

			return Path.Replace("{operationId}", _operationId);
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Get;
        }
    }
}