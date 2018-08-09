using SHLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DMarketSDK.Common.Navigation
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldNavigationElement : UnityBehaviourBase
    {
        private TMP_InputField _inputField;
        private string _previousValue;
        private string _currentValue;

        protected override void Awake()
        {
            base.Awake();
            _inputField = gameObject.GetComponent<TMP_InputField>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _currentValue = _inputField.text;
            _previousValue = null;
            NavigationInputBase.Clicked += OnNavigationClicked;
            _inputField.onEndEdit.AddListener(OnEndEdit);
            _inputField.onSelect.AddListener(OnSelect);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            NavigationInputBase.Clicked -= OnNavigationClicked;
            _inputField.onEndEdit.RemoveListener(OnEndEdit);
            _inputField.onSelect.RemoveListener(OnSelect);
        }

        private void OnSelect(string value)
        {
            _previousValue = value;
        }

        private void OnNavigationClicked(NavigationType type)
        {
            if (gameObject.Equals(EventSystem.current.currentSelectedGameObject))
            {
                if (type == NavigationType.Cancel)
                {
                    _currentValue = _inputField.text;
                    _inputField.text = _previousValue;
                   
                }
                else if (type == NavigationType.Undo)
                {
                    _inputField.text = _currentValue;
                }
            }
        }

        private void OnEndEdit(string value)
        {
            _currentValue = value;
        }
    }
}