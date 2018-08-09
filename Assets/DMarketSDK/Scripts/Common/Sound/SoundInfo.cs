using System;
using UnityEngine;

namespace DMarketSDK.Common.Sound
{
    [Serializable]
    public class SoundInfo
    {
        public MarketSoundType SoundType
        {
            get { return _soundType; }
        }

        public AudioClip Audio
        {
            get { return _audio; }
        }

        public float Volume
        {
            get { return _volume; }
        }

        [SerializeField]
        private MarketSoundType _soundType = MarketSoundType.None;
        [SerializeField]
        private AudioClip _audio = null;
        [SerializeField]
        [Range(0, 1)]
        private float _volume = 1f;
    }
}
