using Lobby;
using ScenesContainer;

namespace States
{
    public abstract class AppLobbySceneBaseState : AppSceneStateBase<LobbySceneController>
    {
        #region implemented abstract members of AppSceneStateBase

        protected override SceneType SceneName
        {
            get
            {
                return SceneType.Lobby;
            }
        }

        #endregion

        protected override void OnSceneStarted()
        {
            SceneController.StartScene(Model.AppType);
        }
    }
}