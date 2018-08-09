using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UnityVersionFixer
{
    /// <summary>
    /// For fixing events for old UI when move version to Unity 2017.1
    /// </summary>
    public class EventSystemFixer : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            StartCoroutine(RefreshEventSystem());
        }

        private IEnumerator RefreshEventSystem()
        {
            yield return new WaitForSeconds(0.3f);
            var eventSystem = FindObjectOfType<EventSystem>();
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            if (eventSystem != null)
            {
                eventSystem.gameObject.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
            if (eventSystem != null)
            {
                eventSystem.gameObject.SetActive(true);
            }

            EventSystem.current.SetSelectedGameObject(selectedObject);
            yield return null;
        }
    }
}