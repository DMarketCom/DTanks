using Game.Decorators;
using Game.Tank;
using TankGame.Domain.GameItem;

namespace Game.Units.Components
{
    public class TankSkinDecorator : IUnitSkinDecorator
    {
        #region IUnitSkinDecorator implementation
        void IUnitSkinDecorator.ApplySkinItem(GameItemType item)
        {
            if(!IsValid(item))
            {
                return;
            }

            var material = _decorator.GetTankMaterial(item);
            if (material != null)
            {
                _tankView.BodyRenderers.ForEach(render =>
                render.sharedMaterial = material);
            }
        }
        #endregion

        private readonly IUnitsSkinCatalog _decorator;
        private readonly TankView _tankView;

        public TankSkinDecorator(IUnitsSkinCatalog decorator, TankView tankView)
        {
            _decorator = decorator;
            _tankView = tankView;
        }

        private bool IsValid(GameItemType itemType)
        {
            return GameItemCategoryExtension.IsValidCategory(itemType, GameItemCategory.Skin);
        }
    }
}