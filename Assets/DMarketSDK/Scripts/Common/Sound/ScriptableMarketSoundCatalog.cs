using System.Collections.Generic;
using SHLibrary.Logging;
using UnityEngine;

namespace DMarketSDK.Common.Sound
{
    [CreateAssetMenu(fileName = "MarketSounds", menuName = "Create/Catalog/MarketSounds")]
    public class ScriptableMarketSoundCatalog : ScriptableObject, IMarketSoundCatalog
    {
        [SerializeField] protected List<SoundInfo> Sounds;

        #region IMarketSoundCatalog implementation
        SoundInfo IMarketSoundCatalog.GetSoundInfo(MarketSoundType sound)
        {
            var result = Sounds.Find(item => item.SoundType == sound);
            if (result == null)
            {
                DevLogger.Warning(string.Format("{0} sound missing", sound),
                    MarketLogType.Sound);
                result = new SoundInfo();
            }

            return result;
        }
#endregion
    }
}