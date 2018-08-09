using System;
using System.Collections;
using DMarketSDK.WidgetCore.Forms;
using SHLibrary;
using SHLibrary.Logging;
using TMPro;
using UnityEngine;

namespace DMarketSDK.Common.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public sealed class MobileInputFieldHandler : UnityBehaviourBase
    {
        public static event Func<InputFieldForm> InputSelected;

        [SerializeField] private string _inputFieldName;
        [SerializeField] private TMP_InputField.InputType _inputType;

        private TMP_InputField _inputField;

        protected override void Awake()
        {
            base.Awake();

            _inputField = GetComponent<TMP_InputField>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _inputField.onSelect.AddListener(OnInputFieldSelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputField.onSelect.RemoveListener(OnInputFieldSelected);
        }

        private void OnInputFieldSelected(string selected)
        {
            if (_inputField.enabled)
            {
                StartCoroutine(WaitFrameAndDeselect());
            }
        }

        private void OnEndInputEdit(string userInput)
        {
            SetTextToInputField(userInput);
        }

        private IEnumerator WaitFrameAndDeselect()
        {
            _inputField.enabled = false;
            yield return new WaitForEndOfFrame();
            _inputField.DeactivateInputField();

            if (InputSelected == null)
            {
                DevLogger.Warning("Event that returns MobileInputForm is null.");
                yield break;
            }

            ShowInputForm(InputSelected.Invoke());
            _inputField.enabled = true;
        }

        private void ShowInputForm(InputFieldForm inputForm)
        {
            var formModel = new InputFieldFormModel
            {
                InputFieldName = _inputFieldName,
                UserInput = _inputField.text,
                InputType = _inputType
            };
            inputForm.ApplyModel(formModel);
            inputForm.Show(OnEndInputEdit);
        }

        private void SetTextToInputField(string text)
        {
            _inputField.onEndEdit.Invoke(text);
        }
    }
}
