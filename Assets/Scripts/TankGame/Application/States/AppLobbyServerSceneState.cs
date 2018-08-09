﻿using TankGame.Network.Server;

namespace TankGame.Application.States
{
    public class AppLobbyServerSceneState : AppLobbySceneBaseState
    {
        protected override void OnSceneStarted()
        {
            base.OnSceneStarted();
            SceneController.LobbyWaitConnection += OnLobbyWaitConnection;
#if APPTYPE_SERVER
            OnLobbySettingCompleted();
#endif
        }

        public override void Finish()
        {
            base.Finish();
            SceneController.LobbyWaitConnection -= OnLobbyWaitConnection;
        }

        private void OnLobbyWaitConnection(string api, int port)
        {
            Controller.Server = new CommonServer();
            if (Controller.Server.Start(port))
            {
                ApplyState<AppRunningServerSceneState>();
            }
        }

        private int GetPortFromConsoleParams()
        {
            int port = 24000;

            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (!string.IsNullOrEmpty(args[i]))
                {
                    string[] param = args[i].Split('=');
                    if (param.Length > 1 
                        && !string.IsNullOrEmpty(param[0]) 
                        && param[0] == "-port"
                    )
                    {
                        int.TryParse(param[1], out port);
                    }
                }
            }

            return port;
        }

        private void OnLobbySettingCompleted()
        {
            Controller.Server = new CommonServer();
            if (Controller.Server.Start(GetPortFromConsoleParams()))
            {
                ApplyState<AppRunningServerSceneState>();
            }
        }
    }
}