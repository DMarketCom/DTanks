using System;
using System.Collections.Generic;
using SHLibrary.ObserverView;
using TankGame.UI.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Authorization
{
    public class AuthorizationView : ObserverViewBase<AuthorizationModel>
    {
        public event Action BackClicked;

        [SerializeField]
        private Button _backButton;

        private readonly List<FormBase> _forms = new List<FormBase>();

        public T GetForm<T>() where T : FormBase
        {
            return _forms.Find(form => form is T) as T;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _backButton.onClick.AddListener(OnBackClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _backButton.onClick.RemoveListener(OnBackClicked);
        }

        private void OnBackClicked()
        {
            BackClicked.SafeRaise();
        }

        public override void ApplyModel(AuthorizationModel model)
        {
            gameObject.GetComponentsInChildren(true, _forms);
            base.ApplyModel(model);
        }

        protected override void OnModelChanged()
        {
            GetForm<LoggedForm>().TxtLogged.text =
                string.Format("Logged: {0}", Model.UserName);
            GetForm <LoginForm>().LoginField.text = Model.UserName;
            GetForm <LoginForm>().PasswordField.text = Model.Password;
            GetForm <RegisterForm>().LoginField.text = Model.UserName;
            GetForm <RegisterForm>().PasswordField.text = Model.Password;
        }
    }
}