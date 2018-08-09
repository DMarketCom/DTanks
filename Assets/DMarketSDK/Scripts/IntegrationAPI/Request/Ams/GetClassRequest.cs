using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Returns class by it's ID
namespace DMarketSDK.IntegrationAPI.Request.Ams
{
    public class GetClassRequest : BaseRequest<GetClassRequest, GetClassRequest.Response, GetClassRequest.RequestParams>
    {
        private const string Path = "/ams/class/{classID}";
		private string _classID = default(string);

        public GetClassRequest(string classID, string gameToken)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			this._classID = classID;
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
			public string description;
			public string id;
			public class ImagesAsset
			{
				public string Image;
				public string Type;
			}
			public List<ImagesAsset> Images;

			public string[] tags;
			public string title;
		}

        protected override string GetBasePath()
        {

			return Path.Replace("{classID}", _classID);
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Get;
        }
    }
}