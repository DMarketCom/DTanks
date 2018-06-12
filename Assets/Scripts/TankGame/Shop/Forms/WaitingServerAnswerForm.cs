using TankGame.Forms;
using TMPro;
using UnityEngine;

namespace Shop.Forms
{
    public class WaitingServerAnswerForm : FormBase
    {
        private const int kMaxAnimFrames = 4;
        
        [SerializeField]
        private TextMeshProUGUI _txtWait;
        [SerializeField]
        private float _timePerAnimFrame = 0.5f;

        private int _animFrameIndex = 0;
        private float _nextUpdateTime = 0;

        protected override float FadeAnimTime
        {
            get
            {
                return 0.5f;
            }
        }

        private void Update()
        {
            if (_nextUpdateTime < Time.timeSinceLevelLoad)
            {
                _nextUpdateTime = Time.timeSinceLevelLoad + _timePerAnimFrame;
                PlayNextAnimFrame();
            }
        }

        private void PlayNextAnimFrame()
        {
            _animFrameIndex++;
            if (_animFrameIndex >= kMaxAnimFrames)
            {
                _animFrameIndex = 0;
            }
            _txtWait.text = "Wait server answer";
            for (int i = 0; i < kMaxAnimFrames; i++)
            {
                _txtWait.text += ".";
            }
        }
    }
}