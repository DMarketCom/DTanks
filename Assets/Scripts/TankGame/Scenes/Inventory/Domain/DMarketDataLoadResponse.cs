using DMarketSDK.IntegrationAPI;
using TankGame.Domain.PlayerData;

namespace TankGame.Inventory.Domain
{
    public class DMarketDataLoadResponse
    {
        public PlayerInventoryInfo Inventory;
        public ErrorCode Error = ErrorCode.None;

        public bool HaveError { get { return Error != ErrorCode.None; } }

        public DMarketDataLoadResponse()
        {

        }

        public DMarketDataLoadResponse(PlayerInventoryInfo inventory, ErrorCode error = ErrorCode.None)
        {
            Inventory = inventory;
            Error = error;
        }
    }
}