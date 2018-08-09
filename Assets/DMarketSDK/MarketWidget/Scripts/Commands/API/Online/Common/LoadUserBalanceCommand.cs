using DMarketSDK.IntegrationAPI.Request.MarketIntegration;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadUserBalanceCommand : ApiCommandBase
    {
        public override void Start()
        {
            base.Start();
            MarketApi.GetPlayerBalance(OnSuccess, OnError);
        }

        private void OnSuccess(GetUserBalanceRequest.Response result, GetUserBalanceRequest.RequestParams request)
        {
            if (IsRunning)
            {
                MarketModel.Balance.Amount = result.amount;
                MarketModel.Balance.Currency = result.currency;
                MarketModel.SetChanges();
                Terminate(true);
            }
        }
    }
}