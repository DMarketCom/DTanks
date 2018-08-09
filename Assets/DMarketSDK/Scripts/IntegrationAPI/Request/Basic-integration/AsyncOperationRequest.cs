using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Get operations statuses details
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class AsyncOperationRequest : BaseRequest<AsyncOperationRequest, AsyncOperationRequest.Response, AsyncOperationRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/async/operation";
		

        public AsyncOperationRequest(string gameToken, string dmarketToken, string[] ids)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				ids = ids
			};
			WithGameToken(gameToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public string[] ids;
		}

        public class Response
		{		
			public class ItemsAsset
			{
				public string assetId;
				public string classId;
				public string operation;
				public string operationId;
				public string status;
				public TransferErrorModel transferError;
			}
			public List<ItemsAsset> Items;

			public int total;
		}

        protected override string GetBasePath()
        {
			return Path;
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Post;
        }
		protected override Dictionary<string, object> GetBody()
		{
			return new Dictionary<string, object>(){
				{"ids", Params.ids}
			};
		}
		
    }
}