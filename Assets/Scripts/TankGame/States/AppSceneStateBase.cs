using UnityEngine.SceneManagement;
using UnityEngine;
using ScenesContainer;

namespace States
{
    public abstract class AppSceneStateBase<T> : AppStateBase
        where T : MonoBehaviour
    {
        protected abstract SceneType SceneName { get; }

        protected T SceneController { get; private set; }

        protected object[] Args { get; private set; }

        public override void Start(object[] args)
        {
            base.Start(args);
            Args = args;
            SceneManager.LoadSceneAsync(GetTargetSceneName(), LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var targetScene = GetTargetSceneName();
            if (scene.name == targetScene)
            {
                SceneController = GameObject.FindObjectOfType<T>();
                Debug.Log(typeof(T));
                OnSceneStarted();
            }
            else
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.UnloadScene(targetScene);
            }
        }

        protected abstract void OnSceneStarted();
        
        private string GetTargetSceneName()
        {
            return Controller.ScenesCatalog.GetSceneName(SceneName);
        }
    }
}