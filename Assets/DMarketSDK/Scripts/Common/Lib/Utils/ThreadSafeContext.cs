using System.Threading;

using SHLibrary.Logging;

using UnityEngine;

namespace SHLibrary.Utils
{
    public static class ThreadSafeContext
    {
        private const int MainThreadId = 1;

        private static readonly bool _isInitialized;

        public static readonly string BuildVersion;

        public static readonly string DeviceUniqueIdentifier;

        public static readonly bool IsEditor;

        public static readonly bool IsWebPlayer;

        public static readonly string PersistentDataPath;

        public static readonly RuntimePlatform Platform;

        static ThreadSafeContext()
        {
            if (Thread.CurrentThread.ManagedThreadId == MainThreadId)
            {
                if (_isInitialized == false)
                {
                    IsWebPlayer = Application.platform == RuntimePlatform.WebGLPlayer;

                    IsEditor = Application.isEditor;

                    BuildVersion = AppVersion;

                    PersistentDataPath = Application.persistentDataPath;

                    DeviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;

                    Platform = Application.platform;

                    _isInitialized = true;
                }
            }
            else
            {
                DevLogger.Warning("ThreadSafeContext should be initialized from the main thread.", LogChannel.Common);
            }
        }

        private static string AppVersion
        {
            get
            {
                string result = Application.version;

#if STAGING
                result = result + "s";
#elif !RELEASE
                result = result + "d";
#endif

                return result;
            }
        }

        public static PlatformType GetPlatformType()
        {
            switch (Platform)
            {
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return PlatformType.Windows;

                case RuntimePlatform.IPhonePlayer:
                    return PlatformType.Ios;

                case RuntimePlatform.Android:
                    return PlatformType.AndroidGooglePlay;

                default:
                    return PlatformType.Undefined;
            }
        }
    }

    public enum PlatformType
    {
        Windows,
        Ios,
        AndroidGooglePlay,
        Undefined
    }
}
