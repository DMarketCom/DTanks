using System;
using TankGame.Domain.PlayerData;

namespace TankGame.GameServer.ServerStorage
{
    public interface IServerStorage
    {
        event Action<string> UserDataChanged;

        bool IsExist(string userName);

        void Add(PlayerInfo data);

        PlayerInfo Get(string userName);

        void Change(PlayerInfo data);

        void Delete(string userName);

        long GetUniqueWorldId();
    }
}