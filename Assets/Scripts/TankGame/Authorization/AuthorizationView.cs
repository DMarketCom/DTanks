using System.Collections.Generic;
using SHLibrary.ObserverView;
using TankGame.Forms;

namespace TankGame.Authorization
{
    public class AuthorizationView : ObserverViewBase<AuthorizationModel>
    {
        private readonly List<FormBase> _forms = new List<FormBase>();

        public T GetForm<T>() where T : FormBase
        {
            return _forms.Find(form => form is T) as T;
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