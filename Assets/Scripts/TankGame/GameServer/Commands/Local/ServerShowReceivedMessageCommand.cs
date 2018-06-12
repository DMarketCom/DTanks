using Networking.Msg;
using Networking.Server;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

namespace GameServer.Commands
{
    public class ServerShowReceivedMessageCommand : ServerCommandBase
    {
        private IGameServer GameServer { get { return Controller.GameServer; } }
        private IAppServer AppServer { get { return Controller.AppServer; } }

        private readonly Text _text;
        private readonly List<GameMsgType> _exeptGameTypes;

        public ServerShowReceivedMessageCommand(Text text)
        {
            _text = text;
            _exeptGameTypes = new List<GameMsgType>(new GameMsgType[] {
                GameMsgType.TankStateUpdate, GameMsgType.UnitMoved
            });
        }

        public override void Start()
        {
            base.Start();
            _text.text = string.Empty;
            AppServer.AppMsgReceived += OnAppMsgReceived;
            GameServer.GameMsgReceived += OnGameMessageReceived;
        }

        protected override void Finish()
        {
            base.Finish();
            AppServer.AppMsgReceived -= OnAppMsgReceived;
            GameServer.GameMsgReceived -= OnGameMessageReceived;
        }

        private void OnGameMessageReceived(GameMessageBase message)
        {
            if (!_exeptGameTypes.Contains(message.Type))
            {
                ShowMsg(message.Type.ToString(), message.ClientId, "yellow");
            }
        }

        private void OnAppMsgReceived(AppMessageBase message)
        {
            ShowMsg(message.Type.ToString(), message.ConnectionId, "blue");
        }

        private void ShowMsg(string type, int conId, string color)
        {
            const int maxCount = 5;
            var userName = Model.ConIdToUserName.ContainsKey(conId) ?
                Model.ConIdToUserName[conId] : "connection id " + conId;
            var log = string.Format("{0} from {1}",
               type, userName);
            log = GetColorLog(log, color);
            var allLogs = _text.text.Split('\n');
            var newLogs = new StringBuilder();
            newLogs.Append("Message received:\n\n");
            newLogs.Append(log);
            newLogs.Append("\n\n");
            for (int i = 1; i < maxCount && i < allLogs.Length; i++)
            {
                newLogs.Append(allLogs[i] + "\n\n");
            }
            _text.text = newLogs.ToString();
        }

        private string GetColorLog(string message, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, message);
        }
    }
}
