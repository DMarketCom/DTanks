using TankGame.Domain.GameItem;
using TankGame.Domain.PlayerData;
using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.Inventory
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
        
        protected override void OnAppMsgReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.Login:
                    OnLoginReceived(message as LoginMessage);
                    break;
                case AppMsgType.Registration:
                    OnRegistrationReceived(message as RegistrationMessage);
                    break;
                case AppMsgType.Logout:
                    OnLogoutReceived(message as LogoutMessage);
                    break;
            }
        }

        private void OnLoginReceived(LoginMessage message)
        {
            var answer = new LoginAnswerMessage();
            NetworkMessageErrorType validationError;

            if (ValidateLoginMessage(message, out validationError))
            {
                answer.PlayerInfo = Storage.Get(message.UserName);
                Model.AddUserSession(message.ConnectionId, message.UserName);
            }
            else
            {
                answer.Error = validationError;
            }

            SendMessageToClient(answer, message.ConnectionId);
        }

        private void OnLogoutReceived(LogoutMessage message)
        {
            Model.RemoveUserSession(message.ConnectionId);
        }

        protected override void OnDisconnected(int connectionId)
        {
            base.OnDisconnected(connectionId);
            Model.RemoveUserSession(connectionId);
        }

        private void OnRegistrationReceived(RegistrationMessage message)
        {
            var answer = new RegistrationAnswerMessage();
            NetworkMessageErrorType errorType;

            if (ValidateRegistrationMessage(message, out errorType))
            {
                PlayerInfo playerData = new PlayerInfo
                {
                    AuthInfo = new PlayerAuthInfo(message.UserName, message.Password)
                };
                PlayerItemInfo defaultSkinItem = new PlayerItemInfo(GameItemType.SkinGreen, Storage.GetUniqueWorldId());
                PlayerItemInfo defaultHelmetItem = new PlayerItemInfo(GameItemType.HelmetPropCap, Storage.GetUniqueWorldId());

                playerData.Inventory.Items.Add(defaultSkinItem);
                playerData.Inventory.EquipItem(defaultSkinItem);

                playerData.Inventory.Items.Add(defaultHelmetItem);
                playerData.Inventory.EquipItem(defaultHelmetItem);

                Storage.Add(playerData);
            }
            else
            {
                answer.Error = errorType;
            }

            SendMessageToClient(answer, message.ConnectionId);
        }

        private void OnStorageDataChanged(string userName)
        {
            int userConnectionId = Model.GetConnectionIdByUserName(userName);
            var playerInfo = Storage.Get(userName);
            var message = new AppUpdatePlayerDataAnswerMessage(playerInfo);
            SendMessageToClient(message, userConnectionId);
        }

        private bool ValidateRegistrationMessage(RegistrationMessage message, out NetworkMessageErrorType errorType)
        {
            bool validationResult = true;
            errorType = NetworkMessageErrorType.None;

            if (string.IsNullOrEmpty(message.UserName))
            {
                errorType = NetworkMessageErrorType.UserNameCannotBeEmpty;
                validationResult = false;
            }
            else if (Storage.IsExist(message.UserName))
            {
                errorType = NetworkMessageErrorType.UserNameBusy;
                validationResult = false;
            }
            else if (string.IsNullOrEmpty(message.Password))
            {
                errorType = NetworkMessageErrorType.UserPasswordCannotBeEmpty;
                validationResult = false;
            }

            return validationResult;
        }

        private bool ValidateLoginMessage(LoginMessage message, out NetworkMessageErrorType errorType)
        {
            bool validationResult = true;
            errorType = NetworkMessageErrorType.None;

            if (message.UserName == string.Empty)
            {
                errorType = NetworkMessageErrorType.UserNameCannotBeEmpty;
                validationResult = false;
            }
            else if (!Storage.IsExist(message.UserName))
            {
                errorType = NetworkMessageErrorType.UserNameNotRegister;
                validationResult = false;
            }
            else if (Storage.Get(message.UserName).AuthInfo.Password != message.Password)
            {
                errorType = NetworkMessageErrorType.UserPasswordNotCorrect;
                validationResult = false;
            }

            return validationResult;
        }
    }
}