using DMarketSDK.Common.UI;
using UnityEngine;

namespace DMarketSDK.Market.Items.Components
{
    public class ItemServerLockComponent : ItemComponentBase
    {
        [SerializeField]
        private LoadingSpinner _spinner;

        [SerializeField]
        private GameObject _view;

        public override bool IsNeedBlockInput
        {
            get { return Target.Model.IsLockFromServer; }
        }

        public override void ModelUpdate()
        {
            base.ModelUpdate();
            SetShowingState(Target.Model.IsLockFromServer);
        }

        private void SetShowingState(bool value)
        {
            _view.SetActive(value);
            _spinner.SetActiveSpinner(value);
        }
    }
}