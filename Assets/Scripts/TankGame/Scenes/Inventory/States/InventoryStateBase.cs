using SHLibrary.StateMachine;

namespace TankGame.Inventory.States
{
    public class InventoryStateBase : StateBase<InventorySceneController, InventoryView>
    {
        protected InventorySceneModel Model { get { return Controller.Model; } }
    }
}