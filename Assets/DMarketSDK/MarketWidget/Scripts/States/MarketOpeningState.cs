using DMarketSDK.Domain;
using DMarketSDK.Market.Forms;

namespace DMarketSDK.Market.States
{
    public sealed class MarketOpeningState : MarketApiStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            ApplyScreenSettings();

            MarketView.Show();

            if (!SavePrefsManager.GetBool(SavePrefsManager.DMARKET_USER_AGREEMENTS))
            {
                ApplyState<UserAgreementsState>();
                return;
            }

            var isLogged = Controller.IsLogged;
            if (isLogged)
            {
                ApplyState<GameInventoryState>();
            }
            else
            {
                ApplyState<MarketLoginFormState>();
            }
        }

        private void ApplyScreenSettings()
        {
            Controller.SaveGameSettings(ScreenOrientationSettings.GetGameOrientationSettings());
            Controller.ApplyScreenSettings(ScreenOrientationSettings.GetMarketOrientationSettings());
        }
    }
}