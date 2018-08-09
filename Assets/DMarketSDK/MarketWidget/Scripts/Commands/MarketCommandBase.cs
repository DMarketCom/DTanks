using DMarketSDK.Forms;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.Commands
{
    public abstract class MarketCommandBase : ScheduledCommandBase<MarketWidgetController>
    {
        protected WidgetModel MarketModel { get { return Controller.Model; } }
    }
}