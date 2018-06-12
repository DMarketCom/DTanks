using SHLibrary.Logging;
using UnityEngine;

namespace States
{
    public class AppInitState : AppStateBase
    {
        public override void Start(object[] args)
        {
            base.Start(args);

            Application.targetFrameRate = 60;

            DevLogger.IsEnable = true;
            DevLogger.IsNeedLogInConsole = true;
            DevLogger.Header = string.Empty;

            Controller.Model = new AppModel();

#if APPTYPE_CLIENT
            InitClient();
#elif APPTYPE_SERVER
            InitServer();
#else
            ApplyState<AppSelectTypeSceneState>();
#endif
        }

        private void InitClient()
        {
            ApplyState<AppSelectTypeSceneState>();
        }

        private void InitServer()
        {
            Controller.Model.AppType = AppType.Server;
            Controller.Model.SetChanges();
            ApplyState<AppLobbyServerSceneState>();
        }
    }
}