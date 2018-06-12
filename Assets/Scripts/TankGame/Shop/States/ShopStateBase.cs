using SHLibrary.StateMachine;
using TankGame.Shop;

namespace Shop.States
{
    public class ShopStateBase : StateBase<ShopSceneController, ShopView>
    {
        protected ShopModel Model { get { return Controller.Model; } }
    }
}