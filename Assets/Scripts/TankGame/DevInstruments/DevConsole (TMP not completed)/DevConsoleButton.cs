using ScenesContainer;
using SHLibrary;
using SHLibrary.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevInstruments.DevConsole
{
    public class DevConsoleButton : UnityBehaviourBase
    {
        public event Action<DevConsoleButton> Clicked;

        [SerializeField]
        private List<SceneType> _targetScenes;
        [SerializeField]
        private Text _txtLabel;
        [SerializeField]
        private Button _btnAction;
        [SerializeField]
        private KeyCode _hotKey = KeyCode.None;

        public List<SceneType> TargetScenes { get { return _targetScenes; } }

        public void Show(string label, KeyCode hotKey = KeyCode.None)
        {
            gameObject.SetActive(true);
            _hotKey = hotKey;
            _txtLabel.text = string.Format("{0}/n key: {1}", label,
                _hotKey);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_hotKey))
            {
                OnClicked();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnAction.onClick.AddListener(OnClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnAction.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked.SafeRaise(this);
        }
    }
}
