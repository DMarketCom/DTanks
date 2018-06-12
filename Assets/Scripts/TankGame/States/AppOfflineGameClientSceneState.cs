using Game;
using ScenesContainer;

namespace States
{
    public class AppOfflineGameClientSceneState : AppSceneStateBase<GameSceneController>
    {
        protected override SceneType SceneName
        {
            get
            {
                return SceneType.Game;
            }
        }

        protected override void OnSceneStarted()
        {
            SceneController.BackClicked += OnBackClick;
            SceneController.Run(GameMode.Offline, Controller.Client);
        }

        public override void Finish()
        {
            base.Finish();
            SceneController.BackClicked -= OnBackClick;
            SceneController.Shutdown();
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            ApplyState<AppSelectTypeSceneState>();
        }
    }
}