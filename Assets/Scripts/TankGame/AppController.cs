using SHLibrary.StateMachine;
using States;
using Networking.Client;
using Networking.Server;
using ScenesContainer;
using UnityEngine;
using PlayerData;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.Widget;

public class AppController : StateMachineBase
{
    public AppModel Model { get; set; }

    public CommonServer Server { get; set; }

    public CommonClient Client { get; set; }


    [HideInInspector]
    public IServerStorage Storage;
    [SerializeField]
    public ScenesCatalog ScenesCatalog;
    [SerializeField]
    public ServerStorageContainer StorageContainer;
    [SerializeField]
    public Widget Widget;
    [SerializeField]
    public ClientApi WidgetApi;

    protected override void Start()
    {
        base.Start();
        DontDestroyOnLoad(gameObject);
        Storage = StorageContainer.Storage;
        ApplyState<AppInitState>();
    }
}