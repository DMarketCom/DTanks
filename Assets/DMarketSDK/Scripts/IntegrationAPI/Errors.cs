namespace DMarketSDK.IntegrationAPI
{
	public enum ErrorCode
    {
        Unknown = -1,
        CannotResolveDestinationHost = 0,
        RequestTimeout = 408,
        Success = 2000,
        GameCredentialsInvalid = 4001,
        BasicAccessTokenInvalid = 4002,
        BasicAccessTokenExpired = 4003,
        DMarketCredentialsInvalid = 4004,
        DMarketTokenInvalid = 4005,
        DMarketTokenExpired = 4006,
        EmptyBasicAccessToken = 4007,
        EmptyDMarketAccessToken = 4008,
        BasicRefreshTokenInvalid = 4009,
        BasicRefreshTokenExpired = 4010,
        DmarketRefreshTokenInvalid = 4011,
        DmarketRefreshTokenExpired = 4012,
        PasswordNotEnoughStrong = 4013,
        EmailIsInvalid = 4019,
        EmptyGameUserId = 4101,
        EmptyDMarketCredentials = 4102,
        DMarketLoginAlreadyUsed = 4103,
        DMarketAccountNotVerified = 4104,
        EmptyAssetId = 4105,
        EmptyClassId = 4106,
        EmptyAssetClass = 4107,
        InvalidClassId = 4108,
        TokenMismatch = 4109,
        AssetAlreadyLocked = 4110,
        MiningLimitExceeded = 4112,
        AssetNotInMarket = 4201,
        AssetAlreadyInMarket = 4202,
        AssetNotFound = 4301,
        ClassNotFound = 4302,
        SellOfferNotFound = 4400,
        AggregatedClassNotFound = 4401,
        AssetAlreadyOnSale = 4402,
        NotEnoughMoney = 4403,
        OwnProduct = 4404,
        PriceCannotBeZero = 4406,
        SellOfferAlreadyCanceled = 4409,
        SellOfferClosed = 4410,
        EmptySellOfferId = 4411,
        InventoryItemNotFound = 4412,
        Internal = 5000,
        
        Highest
    }

    public class Error
    {
        public int HttpCode;
        public ErrorCode ErrorCode;
        public string ErrorMessage;

        public Error()
        {
            HttpCode = 0;
            ErrorCode = ErrorCode.Unknown;
            ErrorMessage = "Using default error constructing";
        }
    }

    public delegate void ErrorCallback(Error error);
}