using System;
using Newtonsoft.Json;
using SHLibrary.Logging;

namespace DMarketSDK.IntegrationAPI.Internal
{
    public class ErrorRepresentation
    {
        public ErrorCode Code;
        public string Message;
    }

    public class ResultContainer<TResultType> where TResultType : new()
    {
        public static void HandleResults(string rawResponse, int code, out TResultType result, out Error error)
        {
            result = default(TResultType);
            error = null;

            try
            {
                if (IsMissingDataHTTPCode(code))
                {
                    if (!string.IsNullOrEmpty(rawResponse))
                    {
                        result = JsonConvert.DeserializeObject<TResultType>(rawResponse, new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Error
                        });
                    }
                }
                else if (IsSuccessHTTPCode(code))
                {
                    error = null;
                    result = JsonConvert.DeserializeObject<TResultType>(rawResponse);
                }
                else if (code == 0)
                {
                    error = new Error
                    {
                        HttpCode = code,
                        ErrorCode = ErrorCode.CannotResolveDestinationHost
                    };
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<ErrorRepresentation>(rawResponse);

                    error = new Error
                    {
                        HttpCode = code,
                        ErrorCode = response.Code,
                        ErrorMessage = response.Message
                    };
                }
            }
            catch (JsonException e)
            {
                DevLogger.Error(string.Format("Cannot parse {0}. Error: {1}", rawResponse, e.Message),
                    MarketLogType.MarketApi);
                error = new Error
                {
                    ErrorCode = ErrorCode.CannotParseResponse,
                    ErrorMessage = "Cant parse response to destination type"
                };
            }
            catch (Exception e)
            {
                DevLogger.Error(e.Message, MarketLogType.MarketApi);
                error = new Error
                {
                    ErrorCode = ErrorCode.Unknown,
                    ErrorMessage = e.ToString()
                };
            }
        }

        private static bool IsMissingDataHTTPCode(int code)
        {
            return code >= 203 && code <= 206;
        }

        private static bool IsSuccessHTTPCode(int code)
        {
            return code >= 200 && code < 300;
        }
    }
}