using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using System;
using System.Collections.Generic;
using TankGame.Domain.GameItem;
using Random = UnityEngine.Random;

namespace DMarketSDK.Market.Commands.API
{
    public class MockUpLoadItemsCommand : ApiCommandBase, ILoadMarketItemsCommand
    {
        private const float kSimulationTime = 0.1f;
        private const int kTotalItemsPerFilter = 17;

        private static readonly Dictionary<GameItemType, string> _testTypeUrls = new Dictionary<GameItemType, string>
        {
            {GameItemType.SkinBlue, "https://image.ibb.co/btOB3T/Item_Skin_Blue.png"},
            {GameItemType.SkinGreen, "https://image.ibb.co/jKYYHo/Item_Skin_Green.png"},
            {GameItemType.SkinYellow, "https://image.ibb.co/bvjjOT/Item_Skin_Yellow.png"},
            {GameItemType.SkinFiolet, "https://image.ibb.co/hqRLxo/Item_Skin_Fiolet.png"},
            {GameItemType.SkinGray, "https://image.ibb.co/ga1Lxo/Item_Skin_Gray.png"},
            {GameItemType.SkinRed, "https://image.ibb.co/gUxSco/Item_Skin_Red.png"},
            {GameItemType.HelmetViking, "https://image.ibb.co/e4ujOT/Item_Helmet_Viking.png"},
            {GameItemType.HelmetPropCap, "https://image.ibb.co/httdiT/Item_Helmet_Prop.png"},
            {GameItemType.HelmetGlasses, "https://image.ibb.co/bRZvV8/Item_Helmet_Glasses.png"},
            {GameItemType.HelmetMilitary, "https://image.ibb.co/myKUq8/Item_Helmet_Military.png"}
        };

        private readonly SellOfferStatusType[] _offerStatuses =
        {
            SellOfferStatusType.Active,
            SellOfferStatusType.Canceled,
            SellOfferStatusType.Failed
        };

        protected readonly ShowingItemsInfo _loadParameters;

        public LoadMarketItemsCommandResult CommandResult { get; private set; }

        public MockUpLoadItemsCommand(ShowingItemsInfo loadParameters)
        {
            _loadParameters = loadParameters;
        }

        public override void Start()
        {
            base.Start();
            ScheduledUpdate(kSimulationTime);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            var items = CreateItems();
            items = ApplySearchFilter(items);
            var totalItems = kTotalItemsPerFilter * (_loadParameters.Categories.Count + 1);
            CommandResult = new LoadMarketItemsCommandResult(items, totalItems);
            Terminate(true);
        }

        protected virtual List<MarketItemModel> CreateItems()
        {
            var result = new List<MarketItemModel>();
            for (int i = 0; i < _loadParameters.Limit; i++)
            {
                var categoryType = Random.value > 0.5f ? GameItemCategory.Skin : GameItemCategory.Helmet;
                var randomItem = GameItemTypeExtensions.GetRandomItem(categoryType);
                int randomStatusIndex = Random.Range(0, _offerStatuses.Length);

                var itemModel = new MarketItemModel
                {
                    Tittle = randomItem.ToString(),
                    ImageUrl = GetUrlForIcon(randomItem),
                    Created = DateTime.Now.AddMinutes(Random.Range(0, 55)),
                    Updated = DateTime.Now.AddMinutes(Random.Range(0, 55)),
                    Fee = { Amount = Random.Range(10, 3000) },
                    Price = { Amount = Random.Range(10, 3000) },
                    AssetId = Random.Range(10000, 100000).ToString(),
                    ClassId = randomItem.ToString(),
                    SellOfferId = Guid.NewGuid().ToString(),
                    IconSprite = null,
                    OffersCount = Random.Range(2, 22),
                    Status = _offerStatuses[randomStatusIndex]
                };
                result.Add(itemModel);
            }
            return result;
        }

        private List<MarketItemModel> ApplySearchFilter(List<MarketItemModel> items)
        {
            return items.FindAll(item => !_loadParameters.HaveSearchFilter ||
            item.Tittle.Contains(_loadParameters.SearchPattern));
        }

        protected string GetUrlForIcon(GameItemType itemType)
        {
            return _testTypeUrls[itemType];
        }
    }
}