using System.Collections.Generic;
using DG.Tweening;
using DMarketSDK.Common.Forms;
using DMarketSDK.Common.Forms.AnimationComponents;
using SHLibrary;
using UnityEngine;

namespace DMarketSDK.Market.PopUps
{
    public class MarketPopUpContainer : UnityBehaviourBase
    {
        [SerializeField]
        private float _popUpDuration = 15f;
        [SerializeField]
        private float _animTime = 0.7f;
        [SerializeField]
        private RectTransform _hidingPopUpPos;
        [SerializeField]
        private MarketTextPopUp _textPopUp;
        

        public void ShowSimpleNotification(string message)
        {
            _textPopUp.Show(message, _popUpDuration);
        }

        public void CloseAll()
        {
            _textPopUp.Hide();
        }

        public void Initialize(MarketTextPopUp textPopUp)
        {
            _textPopUp = textPopUp;

            _textPopUp.ApplyAnimComponent(GetAnimComponents(_textPopUp.gameObject));
        }

        private List<IFormAnimationComponent> GetAnimComponents(GameObject gameObject)
        {
            var result = new List<IFormAnimationComponent>();
            var scaleParams = new TweenAnimParameters(gameObject, _animTime,
                Ease.InExpo, Ease.OutExpo);
            result.Add(new ScaleAnimationComponent(scaleParams));
            var jumpParams = new TweenAnimParameters(gameObject, _animTime,
                Ease.InOutBack, Ease.InOutBack);
            result.Add(new JumpAnimationComponent(_hidingPopUpPos.transform.position, jumpParams));

            return result;
        }
    }
}