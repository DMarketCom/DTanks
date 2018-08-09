using System.Collections.Generic;
using SHLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DMarketSDK.Common.Sound
{
    public class MarketSoundManager : UnityBehaviourBase, IMarketSoundManager
    {
        #region  IMarketSoundManager implementation

        public void Play(MarketSoundType sound)
        {
            if (!IsMuted)
            {
                var soundInfo = Catalog.GetSoundInfo(sound);
                var resultVolume = Volume * soundInfo.Volume;
                GetFreeChannel().PlayOneShot(soundInfo.Audio, resultVolume);
            }
        }

        #endregion

        [SerializeField] private List<AudioSource> _audioSources;
        [SerializeField] private ScriptableMarketSoundCatalog _catalog;
        
        public float Volume { get; set; }

        public bool IsRuning { private get; set; }

        private bool IsMuted
        {
            get { return Volume <= 0.01f || !IsRuning; }
        }

        private IMarketSoundCatalog Catalog
        {
            get { return _catalog; }
        }

        public MarketSoundManager()
        {
            Volume = 1f;
            IsRuning = false;
        }

        private AudioSource GetFreeChannel()
        {
            var result = _audioSources.Find(audioSource => !audioSource.isPlaying);
            if (result == null)
            {
                result = _audioSources[0];
            }

            return result;
        }

        private void Update()
        {
            if (IsMuted)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var currentObject = EventSystem.current.currentSelectedGameObject;
                if (currentObject != null)
                {
                    var currentButton = currentObject.GetComponent<Button>();
                    if (currentButton != null)
                    {
                        var targetSound = currentButton.IsInteractable()
                            ? MarketSoundType.Click
                            : MarketSoundType.Disabled;
                        Play(targetSound);
                    }
                }
            }
        }
    }
}
