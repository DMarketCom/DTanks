namespace TankGame.Network.Messages
{
    public enum GameMsgType : short
    {
        UnitMoved = AppMsgType.Highest + 1,
        BulletStarted,
        Died,
        ConnectToBattleRequest,
        ConnectToBattleAnswer,
        OpponentRespawn,
        TankStateUpdate,
        CreateDropItem,
        PickupGameItem,

        Highest
    }
}