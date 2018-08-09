using UnityEngine;
using UnityEditor;

namespace DMarketSDK.Editor
{
    public class MarketPopUpWindow : EditorWindow
    {
        public string LabelText { private get; set; }

        public static void Show(string text)
        {
            var rect = new Rect(Screen.width / 2, Screen.height / 2, 250, 250);
            Show(text, rect);
        }

        public static void Show(string text, Rect screenSpace)
        {
            MarketPopUpWindow window = ScriptableObject.CreateInstance<MarketPopUpWindow>();
            window.position = screenSpace;
            window.LabelText = text;
            window.ShowPopup();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(LabelText, EditorStyles.wordWrappedLabel);
            GUILayout.Space(70);
            if (GUILayout.Button("Ok")) this.Close();
        }
    }
}