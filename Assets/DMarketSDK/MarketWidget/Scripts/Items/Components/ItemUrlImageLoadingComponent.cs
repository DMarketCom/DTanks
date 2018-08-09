using DMarketSDK.Market.SpriteManager;

namespace DMarketSDK.Market.Items.Components
{
    public class ItemUrlImageLoadingComponent : ItemImageLoadingComponent
    {
        private LoadingSpriteComponent _spriteComponent;

        private ISpriteItemContainer SpriteManager
        {
            get { return UrlImageContainer.Instance; }
        }

        public override void ApplyItem(MarketItemView item)
        {
            base.ApplyItem(item);
            _spriteComponent = new LoadingSpriteComponent(SpriteManager);
            _spriteComponent.Updated += OnSpriteUpdated;
        }

        public override void RemoveItem()
        {
            base.RemoveItem();
            _spriteComponent.UnloadPreviousSprite();
            _spriteComponent.Updated -= OnSpriteUpdated;
        }

        public override void ModelUpdate()
        {
            base.ModelUpdate();
            if (!string.IsNullOrEmpty(Target.Model.ImageUrl))
            {
                _spriteComponent.ApplySprite(Target.Model.ImageUrl, TargetImage);
            }
            OnSpriteUpdated();
        }

        private void OnSpriteUpdated()
        {
            ApplyImageChanging();
        }
    }
}