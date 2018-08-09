using TankGame.Application.Commands;
using TankGame.Authorization;
using TankGame.Catalogs.Scene;
using TankGame.Domain.PlayerData;

namespace TankGame.Application.States
{
    public class AppAuthorizationSceneState : AppSceneStateBase<AuthorizationSceneController>
    {
        protected override SceneType SceneName
        {
            get
            {
                return SceneType.Authorization;
            }
        }

        protected override void OnSceneStarted()
        {
            Controller.Client.Disconnected += OnDisconnect;

            SceneController.Login += OnLogin;
            SceneController.PlayClicked += OnPlay;
            SceneController.InventoryClicked += OnInventory;
            SceneController.LogOut += OnLogout;
            SceneController.BackClicked += OnBackClick;

            SceneController.Run(Model.PlayerModel.AuthInfo, Controller.Client);
        }

        public override void Finish()
        {
            base.Finish();
            Controller.Client.Disconnected -= OnDisconnect;
            SceneController.Login -= OnLogin;
            SceneController.PlayClicked -= OnPlay;
            SceneController.InventoryClicked -= OnInventory;
            SceneController.LogOut -= OnLogout;
            SceneController.BackClicked -= OnBackClick;
            SceneController.Shutdown();
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            ApplyState<AppSelectTypeSceneState>();
        }

        private void OnDisconnect()
        {
            ApplyState<AppLobbyClientSceneState>();
        }

        private void OnLogin(PlayerInfo model)
        {
            ApplyCommand(new AppDetectDataChangingCommand());
            Model.PlayerModel = model;
            Model.SetChanges();
        }

        private void OnLogout()
        {
            Model.PlayerModel.AuthInfo.UserName = string.Empty;
            Model.PlayerModel.AuthInfo.Password = string.Empty;
            Model.SetChanges();
        }

        private void OnPlay()
        {
            ApplyState<AppGameClientSceneState>();
        }

        private void OnInventory()
        {
            ApplyState<AppInventorySceneState>();
        }
    }
}