using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Creates or update class and send it to the marketplace
namespace DMarketSDK.IntegrationAPI.Request.Ams
{
    public class ClassRequest : BaseRequest<ClassRequest, ClassRequest.Response, ClassRequest.RequestParams>
    {
        private const string Path = "/ams/class/{classID}";
		private string _classID = default(string);

        public ClassRequest(string classID, string category, string description, ClassImageModel[] images, string title, string gameToken)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			this._classID = classID;
            Params = new RequestParams
            {
				category = category,
				description = description,
				images = images,
				title = title
			};
			WithGameToken(gameToken);
        }

        public class RequestParams
        {
			public string category;
			public string description;
			public ClassImageModel[] images;
			public string title;
		}

        public class Response
		{		
			public string id;
		}

        protected override string GetBasePath()
        {

			return Path.Replace("{classID}", _classID);
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Post;
        }
		protected override Dictionary<string, object> GetBody()
		{
			return new Dictionary<string, object>(){
				{"category", Params.category},
				{"description", Params.description},
				{"images", Params.images},
				{"title", Params.title}
			};
		}
		
    }
}