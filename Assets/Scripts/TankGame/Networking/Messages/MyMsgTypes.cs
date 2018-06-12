using UnityEngine.Networking;

namespace Networking.Msg
{
    public enum AppMsgType : short
    {
        Login = MsgType.Highest + 1,
        LoginAnswer,
        Registration,
        RegistrationAnswer,
        Logout,
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

    public enum GameMsgType : short
    {
        UnitMoved = AppMsgType.Highest + 1,
        BulletStarted,
        Died,
        ConnectToBattleRequest,
        ConnectToBattleAnswer,
        OponentRespawn,
        TankStateUpdate,
        CreateDropItem,
        PickupGameItem,

        Highest
    }
}