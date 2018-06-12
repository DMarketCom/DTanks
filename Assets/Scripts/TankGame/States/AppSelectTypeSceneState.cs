using SelectAppType;
using ScenesContainer;

namespace States
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
            SceneController.ChooseAppType += OnChooseAppType;
            SceneController.PlayOffline += OnPlayOffline;
        }

        public override void Finish()
        {
            base.Finish();
            SceneController.ChooseAppType -= OnChooseAppType;
            SceneController.PlayOffline -= OnPlayOffline;
            SceneController.Shutdown();
        }

        private void OnChooseAppType(AppType appType)
        {
            Controller.Model.AppType = appType;
            Model.SetChanges();
            if (appType == AppType.Client)
            {
                ApplyState<AppLobbyClientSceneState>();
            }
            else if (appType == AppType.Server)
            {
                ApplyState<AppLobbyServerSceneState>();
            }
        }

        private void OnPlayOffline()
        {
            ApplyState<AppOfflineGameClientSceneState>();
        }
    }
}