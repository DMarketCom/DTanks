using System.Collections.Generic;
using SHLibrary;
using UnityEngine;

namespace TankGame.GameClient.Camera
{
    public sealed class CameraEffectsController : UnityBehaviourBase
    {
        [SerializeField]
        private List<MonoBehaviour> _effectsList;

        protected override void Awake()
        {
            base.Awake();

            #if UNITY_ANDROID || UNITY_IOS
            SwitchCameraEffects(false);
            #endif
        }

        private void SwitchCameraEffects(bool isEnabled)
        {
            foreach (var cameraEffect in _effectsList)
            {
                cameraEffect.enabled = isEnabled;
            }
        }
    }
}