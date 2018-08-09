using System.Collections.Generic;        
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DMarketSDK.Common.Navigation.Keyboard
{
    public class KeyboardNavigationInput : NavigationInputBase
    {
        [SerializeField]
        private bool _shiftPressedTest = false;
        [SerializeField]
        private bool _ctrlPressedTest = false;

        private bool IsShiftPressed
        {
            get { return _shiftPressedTest || Input.GetKey(KeyCode.LeftShift) 
                                           || Input.GetKey(KeyCode.RightShift); }
        }

        private bool IsCtrlPressed
        {
            get { return _ctrlPressedTest || Input.GetKey(KeyCode.LeftControl) 
                                          || Input.GetKey(KeyCode.RightControl); }
        }

        private static readonly List<KeyCode> TargetKeyCodes;

        static KeyboardNavigationInput()
        {
            TargetKeyCodes = new List<KeyCode>()
            {
                KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.LeftArrow,
                KeyCode.RightArrow, KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W,
                KeyCode.Return, KeyCode.Z, KeyCode.U, KeyCode.Tab

            };
        }

        protected override void Awake()
        {
            base.Awake();

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            InputLogic();       
        }         

        private void InputLogic()
        {
            if (!Input.anyKeyDown)
            {
                return;
            }

            var pressedKey = GetPressedDownKey();
            var result = NavigationType.None;
            switch (pressedKey)
            {
                case KeyCode.None:
                    break;
                case KeyCode.DownArrow:
                case KeyCode.S:
                    result = IsInputField() ? NavigationType.None :
                        NavigationType.Down;
                    break;
                case KeyCode.UpArrow:
                case KeyCode.W:
                    result = IsInputField() ? NavigationType.None :
                        NavigationType.Up;
                    break;
                case KeyCode.Tab:
                    result = IsShiftPressed ? NavigationType.Up
                        : NavigationType.Down;
                    break;
                case KeyCode.LeftArrow:
                case KeyCode.A:
                    result = IsInputField() ? NavigationType.None :
                        NavigationType.Left;
                    break;
                case KeyCode.RightArrow:
                case KeyCode.D:
                    result = IsInputField() ? NavigationType.None :
                        NavigationType.Right;
                    break;
                case KeyCode.Return:
                    result = IsInputField() ? NavigationType.Down : NavigationType.Entered;
                    break;
                case KeyCode.Z:
                    result = IsCtrlPressed ? NavigationType.Cancel : NavigationType.None;
                    break;
                case KeyCode.U:
                    result = IsCtrlPressed ? NavigationType.Undo : NavigationType.None;
                    break;
            }

            if (result != NavigationType.None)
            {
                SendNavigationEvent(result);
            }
        }

        private KeyCode GetPressedDownKey()
        {
            foreach (var key in TargetKeyCodes)
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }

            return KeyCode.None;
        }

        private bool IsInputField()
        {
            var currentGameObject = EventSystem.current.currentSelectedGameObject;
            if (currentGameObject != null)
            {
                return currentGameObject.GetComponent<InputField>() ||
                       currentGameObject.GetComponent<TMP_InputField>();
            }

            return false;
        }
    }
}