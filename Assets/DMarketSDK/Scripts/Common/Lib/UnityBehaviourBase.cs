using System;
using UnityEngine;

namespace SHLibrary
{
    /// <summary>
    ///     Is the base class every script derives from.
    /// </summary>
    /// <seealso cref="https://docs.unity3d.com/ru/current/ScriptReference/MonoBehaviour.html" />
    public abstract class UnityBehaviourBase : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets a value indicating whether need to call, Update, FixedUpdate, and OnGUI functions.
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

        public Transform Transform { get; private set; }

        public RectTransform RectTransform { get; private set; }

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
            Transform = transform;
            RectTransform = transform as RectTransform;

            if (KeepOnLoad && Application.isPlaying)
            {
                DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        ///     Is called just before the first call to an Update or a FixedUpdate />
        /// </summary>
        protected virtual void Start()
        {
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
        }

        /// <summary>
        ///     This function is called when the behaviour becomes disabled () or inactive.
        /// </summary>
        protected virtual void OnDisable()
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
    }
}
