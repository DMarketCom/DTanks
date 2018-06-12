using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public abstract class WidgetFormStateBase<T> : WidgetStateBase
        where T : WidgetFormBase
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