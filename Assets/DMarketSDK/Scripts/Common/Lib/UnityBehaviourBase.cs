using System;
using System.Collections;
using System.Diagnostics;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace SHLibrary
{
    /// <summary>
    ///     Is the base class every script derives from.
    /// </summary>
    /// <seealso cref="https://docs.unity3d.com/ru/current/ScriptReference/MonoBehaviour.html" />
    public abstract class UnityBehaviourBase : MonoBehaviour
    {
        /// <summary>
        ///     Gets or sets a value indicating whether need to call <see cref="Start" /> , <see cref="Awake" /> ,
        ///     Update, FixedUpdate, and OnGUI functions.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
        }

        protected virtual bool KeepOnLoad
        {
            get { return false; }
        }

        public event Action<UnityBehaviourBase> Destroyed;

        /// <summary>
        ///     Is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            if (KeepOnLoad && Application.isPlaying)
            {
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        ///     Sent to all game objects when the player gets or looses focus.
        /// </summary>
        /// <remarks>
        ///     OnApplicationFocus can be a co-routine, simply use the yield statement in the function.
        /// </remarks>
        protected virtual void OnApplicationFocus(bool focused)
        {
        }

        /// <summary>
        ///     Sent to all game objects when the player pauses.
        /// </summary>
        /// <remarks>
        ///     OnApplicationPause can be a co-routine, simply use the yield statement in the function.
        /// </remarks>
        protected virtual void OnApplicationPause(bool paused)
        {
        }

        /// <summary>
        ///     Sent to all game objects before the application is quit.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
        }

        /// <summary>
        ///     Is called just before the first call to an Update or a FixedUpdate />
        /// </summary>
        protected virtual IEnumerator Start()
        {
            yield return null;
        }

        /// <summary>
        ///     This allows you to quickly pick important objects in your scene in edit mode.
        /// </summary>
        /// <remarks>
        ///     Will use a mouse position that is relative to the Scene WidgetWidgetView.
        /// </remarks>
        protected virtual void OnDrawGizmosInEditor()
        {
        }

        /// <summary>
        ///     Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
        ///     This allows you to quickly pick important objects in your scene.
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            OnDrawGizmosInEditor();
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        /// <remarks>
        ///     OnDestroyObject will only be called on game objects that have previously been active.
        /// </remarks>
        protected virtual void OnDestroyObject()
        {
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.update += OnEditorUpdate;
            }
#endif
        }

        /// <summary>
        ///     This function is called when the behaviour becomes disabled () or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.update -= OnEditorUpdate;
#endif
        }

        protected virtual void OnEditorUpdate()
        {
        }

        /// <summary>
        ///     This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        /// <remarks>
        ///     OnDestroy will only be called on game objects that have previously been active.
        /// </remarks>
        private void OnDestroy()
        {
            OnDestroyObject();
            Destroyed.SafeRaise(this);
        }
    }
}
