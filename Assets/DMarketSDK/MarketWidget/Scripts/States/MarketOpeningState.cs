using DMarketSDK.Basic;
using DMarketSDK.Domain;
using DMarketSDK.Market.Forms;
using UnityEngine;

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

            var isLogged = ((IMarketWidget)Controller).IsLogged;
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
            Controller.ApplyScreenSettings(ScreenOrientationSettings.GetMarketSettings());
        }
    }
}