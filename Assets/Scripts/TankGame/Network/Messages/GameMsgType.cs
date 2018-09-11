namespace TankGame.Network.Messages
{
    public enum GameMsgType : short
    {
        UnitPosition = AppMsgType.Highest + 1,
        BulletStarted,
        UnitDestroy,
        ConnectToBattleRequest,
        ConnectToBattleAnswer,
        OpponentRespawn,
        CreateDropItem,
        PickupGameItem,

        Highest
    }
}