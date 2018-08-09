using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SHLibrary.Logging;
using SimpleFirebaseUnity;
using TankGame.Domain.PlayerData;
using UnityEngine;

namespace TankGame.GameServer.ServerStorage
{
    public class ServerFirebaseStorage : IServerStorage
    {
        protected long _lastWorldId = 0;
        private readonly int _version;

        [SerializeField]
        public List<PlayerInfo> Players = new List<PlayerInfo>();

        public event Action<string> UserDataChanged;

        private readonly string _firebaseEndpoint = "dmarket-example-project.firebaseio.com";
        private readonly string _firebaseKey = "SicJlW2iUxCRKkxDqq47PQ8Bfi9eTPiMB3mAWuKt";
        private readonly string _playersTableName = "Players";

        private Firebase firebase;
        private Firebase PlayersTable;
        private FirebaseObserver _observer;
        private Dictionary<string, PlayerInfo> PlayersDictionary;

        public ServerFirebaseStorage(int fireBaseVersion)
        {
            _version = fireBaseVersion;

            InitFirebase();

            if(PlayersTable != null)
            {
                GetPlayers();
            }
        }

        #region Firebase Integration
        private void InitFirebase()
        {
            firebase = Firebase.CreateNew(_firebaseEndpoint, _firebaseKey);
            PlayersTable = firebase.Child(_playersTableName);
            
            ClearOldVersion();

            PlayersTable.OnGetSuccess += GetOKHandler;
            PlayersTable.OnGetFailed += FailHandler;
            PlayersTable.OnUpdateSuccess += UpdateOKHandler;
            PlayersTable.OnUpdateFailed += FailHandler;
            PlayersTable.OnPushSuccess += PushOKHandler;
            PlayersTable.OnPushFailed += FailHandler;
            PlayersTable.OnDeleteSuccess += DeleteOKHandler;
            PlayersTable.OnDeleteFailed += FailHandler;
        }

        private void ClearOldVersion()
        {
            var oldData = new List<PlayerInfo>();
            foreach (var player in Players)
            {
                if (player.Version != _version)
                {
                    oldData.Add(player);
                }
            }
            foreach (var player in oldData)
            {
                DevLogger.Log(string.Format("Try delete player info for {0} with old version {1}", player.AuthInfo.UserName, player.Version));
                DeletePlayer(player);
            }
            oldData.Clear();
        }

        void UpdateOKHandler(Firebase sender, DataSnapshot snapshot)
        {
            DevLogger.Log("Firebase.UpdateOKHandler");
        }

        void PushOKHandler(Firebase sender, DataSnapshot snapshot)
        {
            DevLogger.Log("Firebase.PushOKHandler");
        }

        void DeleteOKHandler(Firebase sender, DataSnapshot snapshot)
        {
            DevLogger.Log("Firebase.DeleteOKHandler");
        }

        void FailHandler(Firebase sender, FirebaseError err)
        {
            DevLogger.Error("Firebase.FailHandler " + err.Message);
        }

        void GetPlayers()
        {
            PlayersTable.GetValue(string.Empty);
        }

        void GetOKHandler(Firebase sender, DataSnapshot snapshot)
        {
            //DevLogger.Log("GetOKHandler " + snapshot.RawJson);
            Dictionary<string, string> RawDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(snapshot.RawJson);
            PlayersDictionary = new Dictionary<string, PlayerInfo>();
            foreach (KeyValuePair<string, string> entry in RawDictionary)
            {
                PlayerInfo info = JsonConvert.DeserializeObject<PlayerInfo>(entry.Value);
                PlayersDictionary.Add(entry.Key, info);
                Players.Add(info);
            }
        }

        void AddPlayer(PlayerInfo player)
        {
            string json = JsonConvert.SerializeObject(player, Formatting.Indented);
            PlayersTable.Push(json);
        }

        void UpdatePlayer(PlayerInfo player)
        {
            string json = JsonConvert.SerializeObject(player, Formatting.Indented);
            string FirebaseKey = GetFirebaseKey(player);
            PlayersTable.Child(FirebaseKey).SetValue(json);
        }

        void DeletePlayer(PlayerInfo player)
        {
            string FirebaseKey = GetFirebaseKey(player);
            PlayersTable.Child(FirebaseKey).Delete();
        }

        string GetFirebaseKey(PlayerInfo player)
        {
            foreach (KeyValuePair<string, PlayerInfo> entry in PlayersDictionary)
            {
                if (player.AuthInfo.UserName == entry.Value.AuthInfo.UserName) {
                    return entry.Key;
                }
            }
            return string.Empty;
        }
        #endregion

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
            AddPlayer(data);
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
            UpdatePlayer(data);
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
            DeletePlayer(oldData);
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
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
