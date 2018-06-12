using System;
using System.Collections.Generic;
using SHLibrary.ObserverView;
using UnityEngine;

namespace DevInstruments.DevConsole
{
    public class DevConsoleView : ObserverViewBase<DevConsoleModel>
    {
        public Action<int> Clicked;

        [SerializeField]
        private DevConsoleButton _buttonPrefab;
        
        private List<DevConsoleButton> _buttons;

        protected override void Start()
        {
            base.Start();
            _buttonPrefab.Hide();
        }

        public void CreateButton(string value)
        {
            var button = GameObject.Instantiate(_buttonPrefab, _buttonPrefab.transform);
            button.Show(value, GetFreeHotKey());
            _buttons.Add(button);
        }

        protected override void OnModelChanged()
        {
        }

        public void DestroyAllButtons()
        {
            foreach (var button in _buttons)
            {
                button.Hide();
                GameObject.Destroy(button.gameObject);
            }
            _buttons.Clear();
        }

        private KeyCode GetFreeHotKey()
        {
            switch (_buttons.Count)
            {
                case 0:
                    return KeyCode.F2;
                case 1:
                    return KeyCode.F3;
                case 2:
                    return KeyCode.F4;
                case 3:
                    return KeyCode.F5;
                case 4:
                    return KeyCode.F6;
                case 5:
                    return KeyCode.F7;
                default:
                    return KeyCode.None;
            }
        }
    }
}
