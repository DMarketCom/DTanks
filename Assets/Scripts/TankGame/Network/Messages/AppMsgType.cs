using UnityEngine.Networking;

namespace TankGame.Network.Messages
{
    public enum AppMsgType : short
    {
        Login = MsgType.Highest + 1,
        LoginAnswer,
        Registration,
        RegistrationAnswer,
        Logout,
        LoadInventoryBasicIntegration,
        LoadInventoryBasicIntegrationAnswer,
        UnloadInventoryBasicIntegration,
        UpdatePlayerDataAnswer,
        LoadDMarketDataRequest,
        ItemChangingRequest,
        LoadDMarketDataAnswer,
        ItemChangingAnswer,
        GetGameTokenRequest,
        GetGameTokenAnswer,
        UpdateMarketToken,
        DMarketTransactionCompleted,

        Highest
    }
}