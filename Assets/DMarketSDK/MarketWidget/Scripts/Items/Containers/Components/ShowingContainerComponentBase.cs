using DMarketSDK.Market.Domain;
using SHLibrary;
using System;

namespace DMarketSDK.Market
{
    public abstract class ShowingContainerComponentBase : UnityBehaviourBase
    {
        public event Action Changed;

        public abstract void ModifyShowingParams(ref ShowingItemsInfo showingInfo);

        public abstract void ResetShowingParams(ShowingItemsInfo showingInfo);

        protected void ApplyChanging()
        {
            Changed.SafeRaise();
        }
    }
}