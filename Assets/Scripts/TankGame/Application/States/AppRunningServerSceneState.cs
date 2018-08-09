using TankGame.Catalogs.Scene;
using TankGame.GameServer;

namespace TankGame.Application.States
{
    class AppRunningServerSceneState : AppSceneStateBase<ServerSceneController>
    {
        #region implemented abstract members of AppSceneStateBase

        protected override SceneType SceneName
        {
            get
            {
                return SceneType.Server;
            }
        }

        protected override void OnSceneStarted()
        {
            SceneController.Run(Controller.Server, Controller.Server, Controller.Storage);
            SceneController.BtnBack.onClick.AddListener(OnBackClick);
        }

        #endregion

        public override void Finish()
        {
            base.Finish();
            SceneController.Shutdown();
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            ApplyState<AppLobbyServerSceneState>();
        }
    }
}