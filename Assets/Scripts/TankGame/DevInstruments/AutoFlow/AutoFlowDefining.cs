using SHLibrary;
using UnityEngine;

namespace DevInstruments.AutoFlow
{
    public class AutoFlowDefining : UnityBehaviourBase
    {
        private static bool _isAlreadyDefined = false;

        public static bool IsUseAutoFlow { get; private set; }
        public static AutoFlowSettings Settings { get; private set; }

        [SerializeField]
        private bool _isUseAutoFlow = false;
        [SerializeField]
        private AutoFlowSettings _autoFlowSettings;

        protected override void Awake()
        {
            base.Awake();
            if (!_isAlreadyDefined)
            {
                _isAlreadyDefined = true;
                IsUseAutoFlow = _isUseAutoFlow;
                Settings = _autoFlowSettings;
            }
        }
    }
}
