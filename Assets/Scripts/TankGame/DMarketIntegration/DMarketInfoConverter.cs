using PlayerData;
using SHLibrary.Logging;
using System;

namespace DMarketIntegration
{
    public class DMarketInfoConverter
    {
        static DMarketInfoConverter()
        {
        }

        public string GetClassId(GameItemType itemType)
        {
            return itemType.ToString();
        }

        public string GetAssetId(long worldItemId)
        {
            return worldItemId.ToString();
        }

        public GameItemType GetItemType(string classId)
        {
            try
            {
                return (GameItemType)Enum.Parse(typeof(GameItemType), classId);
            }
            catch(Exception e)
            {
                DevLogger.Error("Cannot find game item type for " + classId
                    + " " + e.Message);
                return GameItemType.Unknow;
            }   
        }

        public long GetWorldId(string assetId)
        {
            return Int64.Parse(assetId);
        }
    }
}
