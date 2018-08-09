using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace DMarketSDK.Market.Containers
{
    public sealed class EmptyFormItemsComponent : ContainerComponentBase
    {
        [SerializeField]
        private TextMeshProUGUI _noItemsMessageText;

        [SerializeField]
        private List<GameObject> _elementsForHide;
        
        protected override void OnModelChanged()
        {
            bool needHideElements = Model.TotalItemsCount == 0;

            _noItemsMessageText.gameObject.SetActive(needHideElements);

            foreach (GameObject go in _elementsForHide)
            {
                go.SetActive(!needHideElements);
            }
        }
    }
}