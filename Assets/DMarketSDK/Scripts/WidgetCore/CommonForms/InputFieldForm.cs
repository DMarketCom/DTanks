using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DMarketSDK.WidgetCore.Forms
{
    public class InputFieldForm : WidgetFormViewBase<InputFieldFormModel>
    {
        [SerializeField]
        private TextMeshProUGUI _inputFieldName;
        [SerializeField]
        private TMP_InputField _inputField;

        private Action<string> _endInputEdit;

        protected override void OnModelChanged()
        {
            base.OnModelChanged();

            _inputFieldName.text = FormModel.InputFieldName;
            _inputField.text = FormModel.UserInput;
            _inputField.inputType = FormModel.InputType;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _inputField.onEndEdit.RemoveListener(OnInputEndEdit);
        }

        public void Show(Action<string> onEndEditCallback)
        {
            base.Show();
            _endInputEdit = onEndEditCallback;
            StartCoroutine(WaitFrameAndCall(() => _inputField.Select()));
        }

        private void OnInputEndEdit(string userInput)
        {
            _endInputEdit.SafeRaise(userInput);

            Hide();
        }

        private IEnumerator WaitFrameAndCall(Action callback)
        {
            yield return new WaitForSeconds(0.1f);
            callback.Invoke();
            _inputField.onEndEdit.AddListener(OnInputEndEdit);
        }
    }
}