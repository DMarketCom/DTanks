using DMarketSDK.WidgetCore.Forms;
using UnityEngine;

namespace DMarketSDK.FormsCatalog
{
    public interface IPlatformUIConfig
    {
        Vector2 CanvasReferenceResolution { get; }

        T GetForm<T>() where T : WidgetFormViewBase;
    }
}
