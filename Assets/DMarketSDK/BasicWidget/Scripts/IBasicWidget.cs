using System;

namespace DMarketSDK.Basic
{
    public interface IBasicWidget : IWidgetCore
    {
        /// <summary>
        /// Reports that the user started logout.
        /// It is necessary to be able to make a bulk transfer for the player's game inventory
        /// to DMarket inventory. Required for Android and iOS platforms.
        /// </summary>
        event Action PreLogoutEvent;

        /// <summary>
        /// Open DMarket BasicWidget.
        /// </summary>
        void Open();
    }
}