using DMarketSDK.Domain;
using UnityEngine;

namespace DMarketSDK.Basic.States
{
    public sealed class BasicWidgetOpeningState : BasicWidgetStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            ApplyScreenSettings();

            WidgetView.Show();

            if (!SavePrefsManager.GetBool(SavePrefsManager.DMARKET_USER_AGREEMENTS))
            {
                ApplyState<UserAgreementsState>();
                return;
            }

            if (Controller.IsLogged)
            {
                ApplyState<BasicWidgetLoggedFormState>();
            }
            else
            {
                ApplyState<BasicWidgetLoginFormState>();
            }
        }

        private void ApplyScreenSettings()
        {
            Controller.ApplyScreenSettings(ScreenOrientationSettings.GetMarketSettings());
        }
    }
}