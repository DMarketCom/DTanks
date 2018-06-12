using Networking.Msg;
using PlayerData;

namespace GameServer.Commands
{
    public class ServerAppDataCommand : ServerAppCommandBase
    {
        public override void Start()
        {
            base.Start();
            Storage.UserDataChanged += OnStorageDataChanged;
        }

        protected override void Finish()
        {
            base.Finish();
            Storage.UserDataChanged -= OnStorageDataChanged;
        }

        private void OnStorageDataChanged(string userName)
        {
            foreach(var key in Model.ConIdToUserName.Keys)
            {
                if (Model.ConIdToUserName[key] == userName)
                {
                    var message = new AppUpdatePlayerDataAnswerMessage(Storage.Get(userName));
                    message.ConnectionId = key;
                    Server.Send(message);
                    return;
                }
            }
        }
        
        protected override void OnAppMsgReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.Login:
                    OnLoginMsg(message as LoginMessage);
                    break;
                case AppMsgType.Registration:
                    OnRegistrMsg(message as RegistrationMessage);
                    break;
                case AppMsgType.Logout:
                    OnLogout(message as LogoutMessage);
                    break;
            }
        }

        private void OnLoginMsg(LoginMessage message)
        {
            var answer = new LoginAnswerMessage();
            if (message.UserName == string.Empty)
            {
                answer.Error = NetMsgErrorType.UserNameCannotBeEmpty;
            }
            else if (!Storage.IsExist(message.UserName))
            {
                answer.Error = NetMsgErrorType.UserNameNotRegister;
            }
            else if (Storage.Get(message.UserName).Autorziation.Password != message.Password)
            {
                answer.Error = NetMsgErrorType.UserPasswordNotCorrect;
            }
            if (!answer.HasError)
            {
                answer.Data = Storage.Get(message.UserName);
                Model.ConIdToUserName.Add(message.ConnectionId, message.UserName);
                Model.SetChanges();
            }
            SendAnswer(answer, message);
        }

        private void OnLogout(LogoutMessage message)
        {
            Model.ConIdToUserName.Remove(message.ConnectionId);
            Model.SetChanges();
        }

        protected override void OnDisconected(int conId)
        {
            base.OnDisconected(conId);
            if (Model.ConIdToUserName.ContainsKey(conId))
            {
                Model.ConIdToUserName.Remove(conId);
            }
        }

        private void OnRegistrMsg(RegistrationMessage message)
        {
            var answer = new RegistrationAnswerMessage();
            if (message.UserName == string.Empty)
            {
                answer.Error = NetMsgErrorType.UserNameCannotBeEmpty;
            }
            else if (Storage.IsExist(message.UserName))
            {
                answer.Error = NetMsgErrorType.UserNameBusy;
            }
            else if (message.Password == string.Empty)
            {
                answer.Error = NetMsgErrorType.UserPasswordCannotBeEmpty;
            }
            if (!answer.HasError)
            {
                PlayerInfo playerData = new PlayerInfo
                {
                    Autorziation = new PlayerAutorizationInfo(message.UserName, message.Password)
                };
                PlayerItemInfo defaultSkinItem = new PlayerItemInfo(GameItemType.SkinGreen, Storage.GetUniqueWorldId());
                PlayerItemInfo defaultHelmetItem = new PlayerItemInfo(GameItemType.HelmetPropCap, Storage.GetUniqueWorldId());

                playerData.Inventory.Items.Add(defaultSkinItem);
                playerData.Inventory.EquipItem(defaultSkinItem);

                playerData.Inventory.Items.Add(defaultHelmetItem);
                playerData.Inventory.EquipItem(defaultHelmetItem);

                Storage.Add(playerData);
            }
            SendAnswer(answer, message);
        }
    }
}