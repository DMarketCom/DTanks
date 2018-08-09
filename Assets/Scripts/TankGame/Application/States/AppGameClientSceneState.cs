using Game;
using TankGame.Catalogs.Scene;

namespace TankGame.Application.States
{
    public class AppGameClientSceneState : AppSceneStateBase<GameSceneController>
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
            SceneController.Run(GameMode.Online, Controller.Client);
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
            ApplyState<AppAuthorizationSceneState>();
        }
    }
}