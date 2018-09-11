using System.Collections.Generic;
using System.Reflection;
using SHLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DMarketSDK.Common.Navigation
{
    public class SimpleNavigationController : UnityBehaviourBase, INavigationElement
    {
        private enum NavigationDirectionType
        {
            Horizontal = 0,
            Vertical = 1,
            HorizontalAndVertical = 2
        }

        private enum DirectionType
        {
            None = 0,
            Next = 1,
            Previous = 2
        }

        [SerializeField]
        private List<Selectable> _components;
        [SerializeField]
        private NavigationDirectionType _navigationType;
        [SerializeField]
        private int _firstSelectedComponentIndex = -1;
        [SerializeField]
        private bool _moveByCircle = true;

        private MethodInfo _getFocusElementMethod;

        private bool IsHorizontalNavigationEnable
        {
            get
            {
                return _navigationType == NavigationDirectionType.Horizontal ||
                       _navigationType == NavigationDirectionType.HorizontalAndVertical;
            }
        }

        private bool IsVerticalNavigationEnable
        {
            get
            {
                return _navigationType == NavigationDirectionType.Vertical
                       || _navigationType == NavigationDirectionType.HorizontalAndVertical;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            NavigationInputBase.Clicked += OnNavigationClicked;
            NavigationRegister.Add(this);
            //TODO crutch: refactoring to navigation creating
            if (Application.isMobilePlatform)
            {
                this.enabled = false;
                return;
            }
            TrySelectCurrentElement();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            NavigationInputBase.Clicked -= OnNavigationClicked;
            NavigationRegister.Remove(this);
        }

        private Selectable TrySelectCurrentElement()
        {
            if (EventSystem.current == null)
            {
                return null;
            }
            if (_firstSelectedComponentIndex >= 0 && _firstSelectedComponentIndex < _components.Count)
            {
                var current = _components[_firstSelectedComponentIndex];
                SelectElement(current);
                return current;
            }

            return null;
        }

        private void SelectElement(Selectable element)
        {
            var targetObject = element == null ? null : element.gameObject;
            EventSystem.current.SetSelectedGameObject(targetObject);          
        }

        protected override void Update()
        {
            base.Update();
            var focusedElement = GetCurrentElementInFocus();
            if (focusedElement != null && _components.Contains(focusedElement))
            {
                var selectedElement = GetCurrentElement();
                if (selectedElement != null && selectedElement != focusedElement)
                {
                    SelectElement(null);
                }
            }
        }

        private Selectable GetCurrentElementInFocus()
        {
            Selectable result = null;
            if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
            {
                return null;
            }
            var inputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
            if (inputModule == null)
            {
                return result;
            }

            if (_getFocusElementMethod == null)
            {
                //TODO wait unity adding aditional instruments for focus object control
                _getFocusElementMethod = typeof(StandaloneInputModule).GetMethod("GetCurrentFocusedGameObject", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            var currentGameObject = _getFocusElementMethod.Invoke(inputModule, null) as GameObject;
            while (currentGameObject != null && result == null)
            {
                result = currentGameObject.GetComponent<Selectable>();
                var parent = currentGameObject.transform.parent;
                currentGameObject = parent == null ? null : parent.gameObject;
            }       
            return result;      
        }

        private void OnEnterClicked()
        {
            var currentElement = GetCurrentElement();
            if (currentElement == null)
            {
                return;
            }

            var button = currentElement as Button;
            if (button != null)
            {
                button.onClick.Invoke();
                return;
            }

            var toggle = currentElement as Toggle;
            if (toggle != null)
            {
                toggle.isOn = !toggle.isOn;
                toggle.onValueChanged.Invoke(toggle.isOn);
                return;
            }
        }

        private void OnNavigationClicked(NavigationType type)
        {
            //TODO refactoring to chain handling
            if (!NavigationRegister.IsCurrent(this))
            {
                return;
            }
            if (type == NavigationType.Entered)
            {
                OnEnterClicked();
                return;
            }

            var focusedObject = GetCurrentElementInFocus();
            if (focusedObject != null && _components.Contains(focusedObject))
            {
                return;
            }

            var targetDirection = DirectionType.None;
            switch (type)
            {
                case NavigationType.Left:
                    targetDirection = IsHorizontalNavigationEnable ? DirectionType.Previous : DirectionType.None;
                    break;
                case NavigationType.Right:
                    targetDirection = IsHorizontalNavigationEnable ? DirectionType.Next : DirectionType.None;
                    break;
                case NavigationType.Up:
                    targetDirection = IsVerticalNavigationEnable ? DirectionType.Previous : DirectionType.None;
                    break;
                case NavigationType.Down:
                    targetDirection = IsVerticalNavigationEnable ? DirectionType.Next : DirectionType.None;
                    break;
            }
            if (targetDirection != DirectionType.None)
            {
                SelectAnotherComponent(targetDirection);
            }
        }

        private void SelectAnotherComponent(DirectionType direction)
        {
            var current = GetCurrentElement();

            if (current == null)
            {
                TrySelectCurrentElement();
                return;
            }

            var currentIndex = _components.IndexOf(current);
            var lastIndex = _components.Count - 1;
            var firstIndex = 0;
            if (direction == DirectionType.Next)
            {
                currentIndex++;
                if (currentIndex > lastIndex)
                {
                    currentIndex = _moveByCircle ? firstIndex : lastIndex;
                }
            }
            else if (direction == DirectionType.Previous)
            {
                currentIndex--;
                if (currentIndex < firstIndex)
                {
                    currentIndex = _moveByCircle ? lastIndex : firstIndex;
                }
            }

            if (_components[currentIndex].gameObject.activeInHierarchy)
            {
                SelectElement(_components[currentIndex]);                                           
            }

        }

        private Selectable GetCurrentElement()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null)
            {
                return null;
            }
            return _components.Find(item => item.gameObject.Equals(selectedGameObject));
        }
    }
}