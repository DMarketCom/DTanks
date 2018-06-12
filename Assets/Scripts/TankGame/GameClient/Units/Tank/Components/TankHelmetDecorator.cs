using PlayerData;
using Game.Tank;
using Game.Decorators;
using Game.Units.Components;
using UnityEngine;

public class TankHelmetDecorator : IUnitSkinDecorator
{
    private IUnitHelmetCatalog _helmetCatalog;
    private TankView _tankView;

    public TankHelmetDecorator(IUnitHelmetCatalog helmetCatalog, TankView tankView)
    {
        _helmetCatalog = helmetCatalog;
        _tankView = tankView;
    }

    #region IUnitSkinDecorator

    public void ApplySkinItem(GameItemType itemType)
    {
        if (!IsValid(itemType))
        {
            return;
        }

        if (_tankView.Helmet != null)
            Object.Destroy(_tankView.Helmet);

        GameObject helmetPrefab = _helmetCatalog.GetHelmet(itemType);
        GameObject helmetInstance = Object.Instantiate(helmetPrefab, _tankView.HelmetPoint);

        _tankView.Helmet = helmetInstance;
    }

    #endregion

    private bool IsValid(GameItemType itemType)
    {
        return GameItemCategoryExtension.IsValidCategory(itemType, GameItemCategory.Helmet);
    }
}