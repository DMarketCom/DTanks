using DMarketSDK.IntegrationAPI.Settings;
using SHLibrary.Logging;
using TankGame.DefineDirectives;

namespace TankGame.Application.States
{
    public class AppInitState : AppStateBase
    {
        public override void Start(object[] args)
        {
            base.Start(args);

            UnityEngine.Application.targetFrameRate = 60;

            DevLogger.IsEnable = true;
            DevLogger.IsNeedLogInConsole = true;
            DevLogger.Header = string.Empty;

            Controller.Model = new AppModel();

            SetMarketEnvironment();

            ApplyState<AppSelectTypeSceneState>();
        }

        private void SetMarketEnvironment()
        {
            var targetEnvironment = Controller.MarketSettings.TargetEnvironment;
            switch (ProjectDefineHelper.TargetEnvironment)
            {
                case EnvironmentDirective.ProductionSandbox:
                    targetEnvironment = EnvironmentType.ProductionSandbox;
                    break;
            }
            Controller.MarketSettings.TargetEnvironment = targetEnvironment;
        }
    }
}