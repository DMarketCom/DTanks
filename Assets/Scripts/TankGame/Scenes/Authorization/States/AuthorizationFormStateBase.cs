using TankGame.UI.Forms;

namespace TankGame.Authorization.States
{
    public abstract class AuthorizationFormStateBase<T> : AuthorizationStateBase
        where T : FormBase
    {
        protected T FormView { get; private set; }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView = View.GetForm<T>();
            FormView.Show();
        }

        public override void Finish()
        {
            base.Finish();
            FormView.Hide();
        }
    }
}