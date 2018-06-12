using UnityEngine;
using SHLibrary;
using TMPro;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// Changes text parameters depending on Toggle state.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleHighlightText : UnityBehaviourBase
    {
        [SerializeField]
        private TextMeshProUGUI _targetText;

        [SerializeField]
        private Color _normalColor;

        [SerializeField]
        private Color _highlightedColor;

        private Toggle _targetToggle;

        protected override void Awake()
        {
            _targetToggle = GetComponent<Toggle>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _targetToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _targetToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            Color textColor = isOn ? _highlightedColor : _normalColor;

            _targetText.color = textColor;
        }
    }
}