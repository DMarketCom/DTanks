using System;
using System.Collections.Generic;
using UnityEngine;

namespace SHLibrary.Logging
{
    public static class DevLogger
    {
        public static event Action<string, LogType> Logged;
        
        private static readonly List<ILogFilter> _filters = new List<ILogFilter>();
        private static ILogColorSetter _logColorSetter;

        public static bool IsEnable = true;
        public static bool IsNeedLogInConsole = true;
        public static string Header = string.Empty;

        public static void AddFilter(ILogFilter filter)
        {
            _filters.Add(filter);
        }

        public static void ApplyLogColorSetter(ILogColorSetter colorSetter)
        {
            _logColorSetter = colorSetter;
        }

        public static void Log(string message, string chanel, LogType logType = LogType.Log)
        {
            DevLog(message, chanel, logType);
        }

        public static void Log(string message, string chanel = LogChannel.Undefined)
        {
            DevLog(message, chanel, LogType.Log);
        }

        public static void Warning(string message, string chanel = LogChannel.Undefined)
        {
            DevLog(message, chanel, LogType.Warning);
        }

        public static void Error(string message, string chanel = LogChannel.Undefined)
        {
            DevLog(message, chanel, LogType.Error);
        }

        public static void Error(Exception exception, string chanel = LogChannel.Undefined)
        {
            if (exception != null)
            {
                DevLog(exception.ToLogString(), chanel, LogType.Error);
            }
        }

        private static void DevLog(string message, string channel, LogType logType)
        {
            if (!IsEnable)
            {
                return;
            }
            foreach (var filter in _filters)
            {
                if (!filter.IsNeedShow(channel, logType))
                {
                    return;
                }
            }
            var resultMessage = string.Format("<b>[{0}][{1:HH:mm:ss:fff}]</b> {2}", channel, DateTime.Now, message);
            if (!string.IsNullOrEmpty(Header))
            {
                resultMessage = string.Format("{0}:   {1}", Header, resultMessage);
            }
            if (_logColorSetter != null)
            {
                var color = _logColorSetter.GetColor(channel);
                var colorHexCode = ToRGBHex(color);
                resultMessage = string.Format("<color=\"{0}\">{1}</color>", colorHexCode, resultMessage);
            }
            if (IsNeedLogInConsole)
            {
                LogInConsole(resultMessage, logType);
            }
            Logged.SafeRaise(resultMessage, logType);
        }

        private static void LogInConsole(string message, LogType logType)
        {
            switch (logType)
            {
                case LogType.Assert:
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Error:
                case LogType.Exception:
                    Debug.LogError(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
            }
        }

        private static string ToRGBHex(Color c)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
    }
}