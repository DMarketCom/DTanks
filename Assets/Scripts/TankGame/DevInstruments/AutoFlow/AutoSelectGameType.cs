using SelectAppType;

namespace DevInstruments.AutoFlow
{
    public class AutoSelectGameType : AutoFlowBase<SelectAppTypeSceneController>
    {
        protected override void ApplyFlowOperation()
        {
            SceneController.SendMessage("OnOnlineClick");
        }
    }
}