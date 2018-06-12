using Shop.DMarketIntegration.States;

namespace Shop.States
{
    public class ShopInitState : ShopStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            Controller.View.Hide();
            if (Controller.Widget.IsLogged)
            {
                ApplyState<ShopLoadBasicWidgetDataState>();
            }
            else
            {
                ApplyState<ShopIdleState>();
            }
        }
    }
}