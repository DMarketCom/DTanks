using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DMarketSDK.Demo
{
    public class SimpleTestConsole : MonoBehaviour
    {
        [SerializeField] private Button _btnLogsEnabling;
        [SerializeField] private TextMeshProUGUI _txtLogs;
        [SerializeField] private int _maxLogsCount = 20;
        [SerializeField] private int _maxOneLogLength = 70;
        [SerializeField] private RectTransform _rootForLogs;

        private readonly List<string> _logs = new List<string>();
        private readonly Dictionary<LogType, string> _logToColor =
            new Dictionary<LogType, string>
            {
                {LogType.Log, "#ffffff"},
                {LogType.Warning, "#ffff33"},
                {LogType.Exception, "#cc2900" },
                {LogType.Error, "#801a00" },
                {LogType.Assert, "#1a8cff" }
            };

        private void Update()
        {
            if (Input.anyKeyDown && PanelEnabling)
            {
                PanelEnabling = false;
            }
        }

        private bool PanelEnabling
        {
            set { _rootForLogs.gameObject.SetActive(value); }
            get { return _rootForLogs.gameObject.activeInHierarchy; }
        }

        private void OnEnable()
        {
            _btnLogsEnabling.onClick.AddListener(OnLogClicked);
            Application.logMessageReceived += OnLogReceived;

        }

        private void OnDisable()
        {
            _btnLogsEnabling.onClick.RemoveListener(OnLogClicked);
            Application.logMessageReceived -= OnLogReceived;
        }

        private void OnLogClicked()
        {
            PanelEnabling = !PanelEnabling;
            UpdateLogsText();
        }

        private void OnLogReceived(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Error)
            {
                PanelEnabling = true;
            }
            var log = condition;
            if (log.Length > _maxOneLogLength)
            {
                log = log.Remove(_maxOneLogLength);
                log += "...";
            }
            if (_logToColor.ContainsKey(type))
            {
                log = string.Format("<color={0}>{1}</color>", _logToColor[type], log);
            }

            _logs.Insert(0, log);
            if (_logs.Count > _maxLogsCount)
            {
                _logs.RemoveAt(_logs.Count - 1);
            }

            UpdateLogsText();
        }

        private void UpdateLogsText()
        {
            if (!PanelEnabling)
            {
                return;
            }

            var allLogsString = new StringBuilder();
            foreach (var log in _logs)
            {
                allLogsString.AppendLine(log);
            }

            _txtLogs.text = allLogsString.ToString();
        }
    }
}