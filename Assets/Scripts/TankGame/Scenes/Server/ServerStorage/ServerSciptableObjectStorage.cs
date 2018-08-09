using System;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Domain.PlayerData;
using UnityEngine;

namespace TankGame.GameServer.ServerStorage
{
    public class ServerSciptableObjectStorage : ScriptableObject, IServerStorage
    {
        private long _lastWorldId = 0;

        [SerializeField]
        public List<PlayerInfo> Players;

        public event Action<string> UserDataChanged;

        #region IServerStorage implementation

        bool IServerStorage.IsExist(string userName)
        {
            foreach (var player in Players)
            {
                if (player.AuthInfo.UserName == userName)
                {
                    return true;
                }
            }
            return false;
        }

        PlayerInfo IServerStorage.Get(string userName)
        {
            return Players.Find(item => item.AuthInfo.UserName.Equals(userName));
        }

        void IServerStorage.Add(PlayerInfo data)
        {
            if (!CorrectDataCheck(data.AuthInfo.UserName, false))
            {
                return;
            }
            Players.Add(data);
            Save();
        }

        void IServerStorage.Change(PlayerInfo data)
        {
            if (!CorrectDataCheck(data.AuthInfo.UserName, true))
            {
                return;
            }
            var oldData = (this as IServerStorage).Get(data.AuthInfo.UserName);
            Players.Remove(oldData);
            Players.Add(data);
            Save();
            UserDataChanged.SafeRaise(data.AuthInfo.UserName);
        }

        void IServerStorage.Delete(string userName)
        {
            if (!CorrectDataCheck(userName, true))
            {
                return;
            }
            var oldData = (this as IServerStorage).Get(userName);
            Players.Remove(oldData);
            Save();
        }

        long IServerStorage.GetUniqueWorldId()
        {
            DateTime centuryBegin = new DateTime(2018, 4, 4);
            DateTime currentDate = DateTime.Now;
            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            var result = (long)elapsedSpan.TotalSeconds;
            if (result <= _lastWorldId)
            {
                result = _lastWorldId + 1;
            }
            _lastWorldId = result;
            return result;
        }

        #endregion

        private bool CorrectDataCheck(string userName, bool mustExist)
        {
            var isExist = (this as IServerStorage).IsExist(userName);
            var errorMessage = string.Empty;
            if (!isExist && mustExist)
            {
                errorMessage = string.Format("{0} not exist in base", userName);
            }
            else if (isExist && !mustExist)
            {
                errorMessage = string.Format("{0} already exist in base", userName);
            }
            if (errorMessage != string.Empty)
            {
                DevLogger.Error(errorMessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Save()
        {
#if	UNITY_2018_1_OR_NEWER
            SetDirty();
#endif
        }
    }
}