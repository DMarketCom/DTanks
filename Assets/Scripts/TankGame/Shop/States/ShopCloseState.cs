using SHLibrary.StateMachine;
using TankGame.Shop;

namespace Shop.States
{
    public class ShopCloseState : StateBase<ShopSceneController>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            Controller.Widget.Close();
        }
    }
}