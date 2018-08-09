using SHLibrary.Logging;

namespace TankGame.DMarketIntegration
{
    public enum DMarketTransactionStatusType
    {
        Pending = 0,
        Success = 1,
        Fail = 2
    }

    public enum DMarketTransactionOperationType
    {
        ToMarket = 0,
        FromMarket = 1
    }

    public class DMarketApiResponseAdapter
    {
        public DMarketTransactionStatusType GetTransactionStatusType(string status)
        {
            switch (status)
            {
                case "processing":
                    return DMarketTransactionStatusType.Pending;
                case "success":
                    return DMarketTransactionStatusType.Success;
                case "fail":
                    return DMarketTransactionStatusType.Fail;
                default:
                    DevLogger.Error("Cannot parse transaction status: " + status);
                    return DMarketTransactionStatusType.Pending;
            }
        }

        public DMarketTransactionOperationType GetTransactionOperationType(string operationType)
        {
            switch (operationType)
            {
                case "toMarket":
                    return DMarketTransactionOperationType.ToMarket;
                case "fromMarket":
                    return DMarketTransactionOperationType.FromMarket;
                default:
                    DevLogger.Error("Cannot parse transaction operation type: " + operationType);
                    return DMarketTransactionOperationType.FromMarket;
            }
        }
    }
}
