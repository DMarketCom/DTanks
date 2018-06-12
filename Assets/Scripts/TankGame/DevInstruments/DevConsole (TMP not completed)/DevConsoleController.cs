using ScenesContainer;
using SHLibrary.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevInstruments.DevConsole
{
    public class DevConsoleController : StateMachineBase<DevConsoleView>
    {
        [SerializeField]
        private KeyCode _turnOnKey = KeyCode.F2;

        private ScenesCatalog _sceneCatalog;

        private bool IsShowing
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }
        

        protected override void Start()
        {
            base.Start();
            _sceneCatalog = Resources.FindObjectsOfTypeAll<ScenesCatalog>()[0];
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(_turnOnKey))
            {
                IsShowing = !IsShowing;
            }
        }
    }
}