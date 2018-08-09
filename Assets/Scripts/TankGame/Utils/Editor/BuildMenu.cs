using TankGame.Application;
using TankGame.Catalogs.Scene;
using UnityEditor;
using UnityEngine;

namespace TankGame.Utils.Editor
{
    public sealed class BuildMenu
    {
        private static string Scenes_path = "Assets/Scenes/";

        private static BuildOptions BuildOptions = BuildOptions.Development;

        [MenuItem("Tools/Build/All/Build All %#b", false, 1)]
        public static void BuildAll()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + getFolder(), AppType.Server, GetPlatform(), GetExtension());
            BuildApp(path + getFolder(), AppType.Client, GetPlatform(), GetExtension());
            BuildApp(path + "/Linux", AppType.Server, BuildTarget.StandaloneLinux64, ".x86_64");
            BuildApp(path + "/Linux", AppType.Client, BuildTarget.StandaloneLinux64, ".x86_64");
#if UNITY_EDITOR_WIN
            BuildApp(path + "/Android", AppType.Client, BuildTarget.Android);
#endif
#if UNITY_EDITOR_OSX
        BuildApp(path + "/iOS", AppType.Client, BuildTarget.iOS);
#endif
            BuildApp(path + "/WebGL", AppType.Client, BuildTarget.WebGL);
        }


#if UNITY_EDITOR_WIN
        [MenuItem("Tools/Build/Windows/Build All", false, 2)]
#endif
#if UNITY_EDITOR_OSX
    [MenuItem("Tools/Build/Mac/Build All", false, 2)]
#endif
        public static void BuildStandaloneAll()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + getFolder(), AppType.Server, GetPlatform(), GetExtension());
            BuildApp(path + getFolder(), AppType.Client, GetPlatform(), GetExtension());
        }

#if UNITY_EDITOR_WIN
        [MenuItem("Tools/Build/Windows/Build Server", false, 3)]
#endif
#if UNITY_EDITOR_OSX
    [MenuItem("Tools/Build/Mac/Build Server", false, 3)]
#endif
        public static void BuildServer()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + getFolder(), AppType.Server, GetPlatform(), GetExtension());
        }

#if UNITY_EDITOR_WIN
        [MenuItem("Tools/Build/Windows/Build Client", false, 4)]
#endif
#if UNITY_EDITOR_OSX
    [MenuItem("Tools/Build/Mac/Build Client", false, 4)]
#endif
        public static void BuildClient()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + getFolder(), AppType.Client, GetPlatform(), GetExtension());
        }

        [MenuItem("Tools/Build/Linux/Build All", false, 5)]
        public static void BuildLinuxAll()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + "/Linux", AppType.Server, BuildTarget.StandaloneLinux, ".x86_64");
            BuildApp(path + "/Linux", AppType.Client, BuildTarget.StandaloneLinux, ".x86_64");
        }

        [MenuItem("Tools/Build/Linux/Build Server", false, 6)]
        public static void BuildLinuxServer()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + "/Linux", AppType.Server, BuildTarget.StandaloneLinux, ".x86_64");
        }

        [MenuItem("Tools/Build/Linux/Build Client", false, 7)]
        public static void BuildLinuxClient()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + "/Linux", AppType.Client, BuildTarget.StandaloneLinux, ".x86_64");
        }

#if UNITY_EDITOR_WIN
        [MenuItem("Tools/Build/Android/Build Client", false, 8)]
        public static void BuildAndroidClient()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + "/Android", AppType.Client, BuildTarget.Android, ".apk");
        }
#endif

#if UNITY_EDITOR_OSX
    [MenuItem("Tools/Build/iOS/Build Client", false, 6)]
    public static void BuildiOSClient()
    {
        var path = GetPath();
        if (string.IsNullOrEmpty(path))
            return;

        BuildApp(path + "/iOS", AppType.Client, BuildTarget.iOS);
    }
#endif

        [MenuItem("Tools/Build/WebGL/Build Client", false, 8)]
        public static void BuildWebGLClient()
        {
            var path = GetPath();
            if (string.IsNullOrEmpty(path))
                return;

            BuildApp(path + "/WebGL", AppType.Client, BuildTarget.WebGL);
        }

        private static void BuildApp(string path, AppType type = AppType.Server, BuildTarget platform = BuildTarget.StandaloneWindows, string extension = "")
        {
            ScenesCatalog scenesCatalog = (ScenesCatalog)Resources.FindObjectsOfTypeAll(typeof(ScenesCatalog))[0];

            string currentDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

            string defineSymbolsForBuild = GetDefineSymbols(type);
            ApplyDefineSymbols(defineSymbolsForBuild);

            BuildPipeline.BuildPlayer(scenesCatalog.GetScenesArray(Scenes_path), path + GetAppFile(type) + extension, platform, BuildOptions);

            ApplyDefineSymbols(currentDefineSymbols);
        }

        private static string GetAppFile(AppType type)
        {
            switch (type)
            {
                case AppType.Server: return "/Server";
                case AppType.Client: return "/Client";
                default: return "/Client";
            }
        }

        private static string GetDefineSymbols(AppType type)
        {
            string defineSymbols = string.Empty;
            switch (type)
            {
                case AppType.Server: defineSymbols = "APPTYPE_SERVER;ENVIRONMENT_QA"; break;
                case AppType.Client: defineSymbols = "APPTYPE_CLIENT;ENVIRONMENT_QA"; break;
                default: defineSymbols = "APPTYPE_CLIENT;ENVIRONMENT_QA"; break;
            }

            return defineSymbols;
        }

        private static void ApplyDefineSymbols(string defineSymbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defineSymbols);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, defineSymbols);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineSymbols);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defineSymbols);
        }

        private static BuildTarget GetPlatform()
        {
#if UNITY_EDITOR_WIN
            return BuildTarget.StandaloneWindows;
#endif
#if UNITY_EDITOR_OSX && UNITY_2017_1
            return BuildTarget.StandaloneOSXIntel64;
#elif UNITY_EDITOR_OSX 
            return BuildTarget.StandaloneOSX;
#endif

        }

        private static string GetExtension()
        {
#if UNITY_EDITOR_WIN
            return ".exe";
#endif
#if UNITY_EDITOR_OSX
            return ".app";
#endif
        }

        private static string getFolder()
        {
#if UNITY_EDITOR_WIN
            return "/Windows";
#endif
#if UNITY_EDITOR_OSX
            return "/Mac";
#endif
        }

        private static string GetPath()
        {
            var prevPath = EditorPrefs.GetString("msf.buildPath", "");
            string path = EditorUtility.SaveFolderPanel("Choose Location for binaries", prevPath, "");

            if (!string.IsNullOrEmpty(path))
            {
                EditorPrefs.SetString("msf.buildPath", path);
            }
            return path;
        }
    }
}