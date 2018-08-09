using System;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Catalogs.Scene
{
    [CreateAssetMenu(fileName = "ScenesCatalog", menuName = "Create/Catalog/ScenesCatalog", order = 1)]
    public class ScenesCatalog : ScriptableObject
    {
        [Serializable]
        public class ScenesRef
        {
            public SceneType Type;
            public string Scene;
        }

        [Header("Scenes list")]
        public List<ScenesRef> Scenes;

        public string GetSceneName(SceneType scene)
        {
            foreach (ScenesRef Scene in Scenes)
            {
                if (Scene.Type == scene)
                {
                    return Scene.Scene;
                }
            }
            return null;
        }

        public SceneType GetSceneType(string sceneName)
        {
            return Scenes.Find(scene => scene.Scene == sceneName).Type;
        }

        public string[] GetScenesArray(string folder)
        {
            List<string> scenesPath = new List<string>();
            foreach (ScenesRef Scene in Scenes)
            {
                scenesPath.Add(folder + Scene.Scene + ".unity");
            }

            return scenesPath.ToArray();
        }
    }
}