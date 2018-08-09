using UnityEngine;

namespace DMarketSDK.IntegrationAPI.Internal
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    
                    DontDestroyOnLoad(go);
                }

                if (_instance.Initialized) return _instance;
                
                _instance.Initialize();
                _instance.Initialized = true;

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
            }
        }

        private bool Initialized { get; set; }

        protected virtual void Initialize()
        { }
    }
}
