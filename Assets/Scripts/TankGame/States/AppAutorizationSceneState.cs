using ScenesContainer;
using PlayerData;
using Commands.Client;
using TankGame.Authorization;

namespace States
{
    public class AppAutorizationSceneState : AppSceneStateBase<AuthorizationSceneController>
    {
        protected override SceneType SceneName
        {
            get
            {
                return SceneType.Autorization;
            }
        }

        protected override void OnSceneStarted()
        {
            var autorizationInfo = Model.PlayerModel.Autorziation;
            SceneController.Run(autorizationInfo.IsLogged, autorizationInfo.UserName,
                autorizationInfo.Password, Controller.Client);
            Controller.Client.Disconected += OnDisconect;
            SceneController.Login += OnLogin;
            SceneController.PlayClicked += OnPlay;
            SceneController.ShopClicked += OnShop;
            SceneController.LogOut += OnLogout;
        }

        public override void Finish()
        {
            base.Finish();
            Controller.Client.Disconected -= OnDisconect;
            SceneController.Login -= OnLogin;
            SceneController.PlayClicked -= OnPlay;
            SceneController.ShopClicked -= OnShop;
            SceneController.LogOut -= OnLogout;
            SceneController.Shutdown();
        }

        private void OnDisconect()
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
            Model.PlayerModel.Autorziation.UserName = string.Empty;
            Model.PlayerModel.Autorziation.Password = string.Empty;
            Model.SetChanges();
        }

        private void OnPlay()
        {
            ApplyState<AppGameClientSceneState>();
        }

        private void OnShop()
        {
            ApplyState<AppShopSceneState>();
        }
    }
}