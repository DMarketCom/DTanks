namespace DMarketSDK.Market.Items.Components
{
    public class ItemIconComponent : ItemImageLoadingComponent
    {
        public override void ModelUpdate()
        {
            base.ModelUpdate();
            TargetImage.sprite = Target.Model.IconSprite;
            ApplyImageChanging();
        }
    }
}
