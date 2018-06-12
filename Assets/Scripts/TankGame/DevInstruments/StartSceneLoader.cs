using SHLibrary;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace DevInstruments
{
    public class StartSceneLoader : UnityBehaviourBase
    {
#if UNITY_EDITOR
        #region Dev test code
        [SerializeField]
        private AppType _targetType = AppType.Client;

        protected override void Awake()
        {
            base.Awake();
            var isNeedLoadStart = GameObject.FindObjectOfType<AppController>() == null;
            if (isNeedLoadStart)
            {
                var targetSceneIndex = _targetType == AppType.Client ? 0 : 1;
                SceneManager.LoadScene(targetSceneIndex);
            }
        }
        #endregion
#endif
    }
}
