using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TankGame.Utils.Editor
{
    internal sealed class SceneSelectorWindow : EditorWindow
    {
        private const float ButtonHeight = 20f;
        private const float ButtonPadding = 5f;
        private const float WindowWidth = 200f;
        private const string WindowTitle = "Scene Selector";
        private const string BoxKey = "box";

        [MenuItem("Tools/Scene Selector %&s")]
        public static void Init()
        {
            SceneSelectorWindow window = GetWindow<SceneSelectorWindow>(false, WindowTitle, true);
            window.Show();
        }

        private List<EditorBuildSettingsScene> _scenes;

        private void OnEnable()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            minSize = new Vector2(WindowWidth, sceneCount * (ButtonHeight + ButtonPadding));
            maxSize = minSize;

            _scenes = new List<EditorBuildSettingsScene>(sceneCount);
            for(int sceneIndex = 0; sceneIndex < sceneCount; ++sceneIndex)
            {
                _scenes.Add(EditorBuildSettings.scenes[sceneIndex]);
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(BoxKey);

            {
                foreach (var scene in _scenes)
                {
                    string sceneName = Path.GetFileName(scene.path);

                    if (!GUILayout.Button(Path.ChangeExtension(sceneName, null), GUILayout.Height(ButtonHeight)))
                        continue;

                    var loadScene = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    if (loadScene)
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}