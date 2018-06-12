using UnityEngine;

namespace DMarketSDK.Common.VisualEffects
{
    public class SimpleRotatingView : MonoBehaviour
    {
        private RectTransform _rectTrnsf;
        private float _currentvalue;

        [SerializeField]
        private float _rotateSpeed = 2f;

        private void Start()
        {
            _rectTrnsf = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _currentvalue = _currentvalue + (Time.deltaTime * _rotateSpeed);

            _rectTrnsf.transform.rotation = Quaternion.Euler(0f, 0f, -72f * (int)_currentvalue);
        }
    }
}