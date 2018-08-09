namespace DMarketSDK.Common.Sound
{
    public interface IMarketSoundCatalog
    {
        SoundInfo GetSoundInfo(MarketSoundType sound);
    }
}