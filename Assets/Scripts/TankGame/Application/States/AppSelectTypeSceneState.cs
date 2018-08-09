using SelectAppType;
using TankGame.Catalogs.Scene;

namespace TankGame.Application.States
{
    public class AppSelectTypeSceneState : AppSceneStateBase<SelectAppTypeSceneController>
    {
        #region implemented abstract members of AppSceneStateBase

        protected override SceneType SceneName
        {
            get
            {
                return SceneType.SelectAppType;
            }
        }

        #endregion

        protected override void OnSceneStarted()
        {
            SceneController.Run();
            SceneController.ChooseClient += InitClient;
            SceneController.ChooseServer += InitServer;
            SceneController.PlayOffline += OnPlayOffline;

#if APPTYPE_SERVER || APPTYPE_WEBSERVER
            InitServer();
#elif APPTYPE_CLIENT
            InitClient();
#endif
        }

        public override void Finish()
        {
            base.Finish();
            SceneController.ChooseClient -= InitClient;
            SceneController.ChooseServer -= InitServer;
            SceneController.PlayOffline -= OnPlayOffline;
            SceneController.Shutdown();
        }
        
        private void InitClient()
        {
            ApplyState<AppLobbyClientSceneState>();
            Controller.Model.AppType = AppType.Client;
            Controller.Model.SetChanges();
            ApplyState<AppLobbyClientSceneState>();
        }

        private void InitServer()
        {
            Controller.Model.AppType = AppType.Server;
            Controller.Model.SetChanges();
            ApplyState<AppLobbyServerSceneState>();
        }

        private void OnPlayOffline()
        {
            ApplyState<AppOfflineGameClientSceneState>();
        }
    }
}