using DMarketSDK;
using DMarketSDK.FormsCatalog;
using UnityEditor;
using UnityEngine;

namespace TankGame.Utils.Editor
{
    public class DevInstrumentsEditor
    {
        [MenuItem("Tools/Dev Instruments/Create Scriptable object")]
        public static void CreateScriptableObject()
        {
            var asset = ScriptableObject.CreateInstance<MarketPlatformUIConfig>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/TODO_addName.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        [MenuItem("Tools/Dev Instruments/Clear Player Preferences")]
        public static void ClearPlayerPreferences()
        {
            SavePrefsManager.DeleteAll();
        }
    }
}
