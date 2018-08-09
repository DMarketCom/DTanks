using DMarketSDK.IntegrationAPI.Internal;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;
using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.IntegrationAPI.Settings;

namespace DMarketSDK.IntegrationAPI
{
    public class ClientApi : BaseApi<IClientApiSettings>
    {
        public string MarketToken { get; set; }

        #region Authorization request
        public void GetMarketAccessToken(
            string basicAccessToken,
            string email,
            string password,
            ResultCallback<TokenRequest.Response, TokenRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new TokenRequest(basicAccessToken, email, password)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void RegisterMarketAccount(
            string basicAccessToken,
            string email,
            string password,
            string username = null,
            ResultCallback<RegisterRequest.Response, RegisterRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new RegisterRequest(basicAccessToken, email, password, username)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void Logout(
            string basicAccessToken,
            string marketAccessToken,
            ResultCallback<LogoutRequest.Response, LogoutRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new LogoutRequest(basicAccessToken, marketAccessToken)
               .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetBasicRefreshToken(
            string basicRefreshToken,
            ResultCallback<BasicRefreshTokenRequest.Response, BasicRefreshTokenRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new BasicRefreshTokenRequest(basicRefreshToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void RestorePassword(
            string basicAccessToken, 
            string email,
            ResultCallback<RestorePasswordRequest.Response, RestorePasswordRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new RestorePasswordRequest(basicAccessToken, email)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }
        #endregion

        #region Basic requests
        public void GetDocument(
           string basicAccessToken,
           string type,
           ResultCallback<GetDocumentRequest.Response, GetDocumentRequest.RequestParams> callback = null,
           ErrorCallback errorCallback = null
       )
        {
            new GetDocumentRequest(type, basicAccessToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }
        #endregion

        #region MarketWidget request
        public void CreateSellOfferRequest(string assetId, long amount, string currency,
            ResultCallback<UserSellOfferRequest.Response, UserSellOfferRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
            new UserSellOfferRequest( assetId, amount, MarketToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void EditSellOfferRequest(string offerId, long amount, string currency,
        ResultCallback<PutUserSellOfferRequest.Response, PutUserSellOfferRequest.RequestParams> callback = null,
        ErrorCallback errorCallback = null)
        {
            new PutUserSellOfferRequest(offerId, amount, MarketToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void CancelSellOfferRequest(string sellOffer,
            ResultCallback<UserSellOfferCancelRequest.Response, UserSellOfferCancelRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
            new UserSellOfferCancelRequest(sellOffer, MarketToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void LoadMarketInventoryRequest(
            int limit, int offset, string query,
            ResultCallback<Request.MarketIntegration.GetUserInventoryRequest.Response,
                Request.MarketIntegration.GetUserInventoryRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
            new Request.MarketIntegration.GetUserInventoryRequest(MarketToken, limit, offset, query)
               .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void LoadSellOffersRequest(
            int limit, int offset, string query,
            string orderBy, string orderDir, string status,
            ResultCallback<GetUserSellOffersRequest.Response,
                GetUserSellOffersRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
            new GetUserSellOffersRequest(status, MarketToken, limit, offset, query,
                orderBy, orderDir)
               .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetGameCategories(ResultCallback<GetGameCategoriesRequest.Response, GetGameCategoriesRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
            new GetGameCategoriesRequest(MarketToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

		public void GetAggregatedSellOffers(GetGameAggregatedClassesRequest.RequestParams args, 
			ResultCallback<GetGameAggregatedClassesRequest.Response,
            GetGameAggregatedClassesRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
			new GetGameAggregatedClassesRequest(MarketToken, args)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void GetAggregatedSellOffersByClassId(string classId, int limit = 0, int offset = 0, ResultCallback<GetGameClassSellOffersRequest.Response, GetGameClassSellOffersRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null)
        {
            new GetGameClassSellOffersRequest(classId, MarketToken, limit, offset)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void BuyItemRequest(string sellOfferId,
                ResultCallback<UserSellOfferBuyRequest.Response, UserSellOfferBuyRequest.RequestParams> callback = null,
                ErrorCallback errorCallback = null)
        {
            new UserSellOfferBuyRequest(sellOfferId, MarketToken)
               .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetBuyOrderByList(ResultCallback<GetSellOfferOrderByListRequest.Response,
    GetSellOfferOrderByListRequest.RequestParams> callback = null,
        ErrorCallback errorCallback = null)
        {
            new GetSellOfferOrderByListRequest(MarketToken)
                .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetSellOrderByList(ResultCallback<GetSellOfferOrderByListRequest.Response,
 GetSellOfferOrderByListRequest.RequestParams> callback = null,
     ErrorCallback errorCallback = null)
        {
            new GetSellOfferOrderByListRequest(MarketToken)
                .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetOrderStatuses(ResultCallback<GetSellOfferStatusesRequest.Response,
            GetSellOfferStatusesRequest.RequestParams> callback = null,
                ErrorCallback errorCallback = null)
        {
            new GetSellOfferStatusesRequest(MarketToken)
                .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetPlayerBalance(ResultCallback<GetUserBalanceRequest.Response,
            GetUserBalanceRequest.RequestParams> callback = null,
                ErrorCallback errorCallback = null)
        {
            new GetUserBalanceRequest(MarketToken)
                .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }
        #endregion
    }
}