using DG.Tweening;
using SHLibrary.ObserverView;
using UnityEngine;

namespace TankGame.Forms
{
    public abstract class FormBase : ViewBase
    {
        protected virtual float FadeAnimTime { get { return 0.7f; } }

        private CanvasGroup _canvasGroup;
        private Tweener _currentAnim;

        protected bool UseAnimation { get { return true; } }

        public override void Show()
        {
            if (UseAnimation)
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.GetComponent<CanvasGroup>();
                    if (_canvasGroup == null)
                    {
                        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    }
                    _canvasGroup.alpha = 0f;
                }
                _canvasGroup.GetComponent<RectTransform>().SetAsLastSibling();
                PlayFadeAnim(true);
            }
            else
            {
                base.Show();
            }
        }

        public override void Hide()
        {
            if (UseAnimation)
            {
                PlayFadeAnim(false);
            }
            else
            {
                base.Hide();
            }
        }

        private void PlayFadeAnim(bool show)
        {
            if (_currentAnim != null)
            {
                _currentAnim.Kill();
            }
            gameObject.SetActive(true);
            _currentAnim = _canvasGroup.DOFade(show ? 1 : 0, FadeAnimTime);
            
            _currentAnim.OnComplete(OnFadeAnimComplete);
            _currentAnim.SetEase(Ease.OutExpo);
        }

        private void OnFadeAnimComplete()
        {
            gameObject.SetActive(_canvasGroup.alpha > 0.1f);
            _currentAnim = null;
        }
    }
}