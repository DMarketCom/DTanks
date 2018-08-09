using System.Text;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;
using UnityEngine;
using DMarketSDK.IntegrationAPI.Settings;
using DMarketSDK.IntegrationAPI.Request;
using System.Collections.Generic;
using DMarketSDK.Basic;
using DMarketSDK.Domain;

#if UNITY_EDITOR
namespace DMarketSDK.Demo
{
    public sealed class BasicWidgetTest : MonoBehaviour
    {
        private string _sellOfferId;
        private List<string> _itemOperationId = new List<string>();

        [Header("DMarket SDK")]
        [SerializeField]
        private ClientApi _clientApi;

        [SerializeField] private ServerApi _serverApi;
        [SerializeField] private BasicWidgetController _basicWidget;
        [SerializeField] private MarketApiSettings _marketApiSettings;

        [Header("DMarket User")]
        [SerializeField]
        private string _dMarketUserEmail;

        [SerializeField] private string _dMarketUserPassword;

        [Header("Your game user Id.")]
        [Tooltip("E.g: SuperUser")]
        [SerializeField]
        private string _gameUserId;

        [Header("MarketWidget item info.")]
        [Tooltip("Unique item instance Id. E.g: 1354679514")]
        [SerializeField]
        private string _itemAssetId;

        [Tooltip("Item type Id. E.g: AK-47")]
        [SerializeField]
        private string _itemClassId;

        [Header("DMarket Tokens.")]
        [Tooltip("You will receive tokens from DMarketApi.")]
        [SerializeField]
        private string _marketAccessToken;

        [Tooltip("You will receive tokens from DMarketApi.")]
        [SerializeField]
        private string _basicAccessToken;

        [Tooltip("You will receive tokens from DMarketApi.")]
        [SerializeField]
        private string _basicRefreshToken;

        [Tooltip("You will receive tokens from DMarketApi.")]
        [SerializeField]
        private string _marketRefreshToken;

        private IBasicWidget BasicWidget
        {
            get { return _basicWidget; }
        }

        #region MonoBehaviour methods

        private void Awake()
        {
            _serverApi.ApplyHttpProtocol(_marketApiSettings);
            _clientApi.ApplyHttpProtocol(_marketApiSettings);
            _itemAssetId = string.IsNullOrEmpty(_itemAssetId)
                ? (_itemAssetId = GetRandomItemId())
                : _itemAssetId;
        }

        private void OnEnable()
        {
            BasicWidget.LoginEvent += OnWidgetLogin;
            BasicWidget.LogoutEvent += OnWidgetLogout;
            BasicWidget.ErrorEvent += OnErrorCallback;
        }

        private void OnDisable()
        {
            BasicWidget.LoginEvent -= OnWidgetLogin;
            BasicWidget.LogoutEvent -= OnWidgetLogout;
            BasicWidget.ErrorEvent -= OnErrorCallback;
        }

        #endregion

        #region DMarket access tokens

        [ContextMenu("GetBasicAccessToken")]
        public void GetBasicAccessToken()
        {
            _serverApi.GetBasicAccessToken(_gameUserId, GetBasicTokenCallback, OnErrorCallback);
        }

        private void GetBasicTokenCallback(BasicTokenRequest.Response response,
            BasicTokenRequest.RequestParams requestParams)
        {
            Debug.Log(string.Format(
                "GetBasicAccessToken request completed. BasicAccessToken: {0} BasicRefreshToken: {1}", response.token,
                response.refreshToken));

            _basicAccessToken = response.token;
            _basicRefreshToken = response.refreshToken;
        }

        [ContextMenu("GetMarketAccessToken")]
        public void GetMarketAccessToken()
        {
            _clientApi.GetMarketAccessToken(_basicAccessToken, _dMarketUserEmail, _dMarketUserPassword,
                GetMarketAccessTokenRequest, OnErrorCallback);
        }

        private void GetMarketAccessTokenRequest(TokenRequest.Response response, TokenRequest.RequestParams request)
        {
            Debug.Log(string.Format(
                "GetMarketAccessTokenRequest request completed. BasicAccessToken: {0} BasicRefreshToken: {1}",
                response.token, response.refreshToken));

            _marketAccessToken = response.token;
            _marketRefreshToken = response.refreshToken;

            _clientApi.MarketToken = _marketAccessToken;
        }

        [ContextMenu("RefreshMarketToken")]
        public void RefreshMarketToken()
        {
            _serverApi.GetMarketRefreshToken(_marketRefreshToken, MarketRefreshTokenCallback, OnErrorCallback);
            Debug.Log("RefreshMarketToken request was sent.");
        }

        private void MarketRefreshTokenCallback(RefreshTokenRequest.Response result,
            RefreshTokenRequest.RequestParams request)
        {
            _marketAccessToken = result.token;
            _marketRefreshToken = result.refreshToken;

            _clientApi.MarketToken = _marketAccessToken;

            Debug.Log("RefreshMarketToken request completed.");
        }

        #endregion

        #region DMarket widget

        /// <summary>
        /// For open DMarketWidget you must receive BasicAccessToken and BasicRefreshToken
        /// from ServerApi. Use GetBasicAccessToken method for receive it.
        /// </summary>
        [ContextMenu("Init and Open Widget")]
        public void OpenWidget()
        {
            _serverApi.GetBasicAccessToken(_gameUserId, OpenWidget_GetBasicAccessTokenCallback, OnErrorCallback);
        }

        private void OpenWidget_GetBasicAccessTokenCallback(BasicTokenRequest.Response result,
            BasicTokenRequest.RequestParams request)
        {
            _basicAccessToken = result.token;
            _basicRefreshToken = result.refreshToken;
            BasicWidget.Init(_basicAccessToken, _basicRefreshToken, _clientApi);
            BasicWidget.Open();
        }

        private void OnWidgetLogin(LoginEventData data)
        {
            Debug.Log(string.Format("UserLogged.MarketAccessToken: {0}\nMarketRefreshAccessToken: {1}",
                data.MarketAccessToken, data.MarketRefreshAccessToken));

            _marketAccessToken = data.MarketAccessToken;
            _marketRefreshToken = data.MarketRefreshAccessToken;

            _clientApi.MarketToken = _marketAccessToken;
        }

        private void OnWidgetLogout()
        {
            Debug.Log("Widget Logout");
        }

        #endregion

        #region DMarketApi methods

        [ContextMenu("ToMarket item")]
        public void ToMarketItem()
        {
            _serverApi.AsyncToMarket(_marketAccessToken, GetTargetItems(), ToMarketCallback, OnErrorCallback);
            Debug.Log("ToMarket request was sent.");
        }

        private AssetToMarketModel[] GetTargetItems()
        {
            return new[] { new AssetToMarketModel() { assetId = _itemAssetId, classId = _itemClassId } };
        }

        private void ToMarketCallback(AsyncMarketResponse result, AsyncToMarketRequest.RequestParams request)
        {
            foreach (var item in result.Items)
            {
                _itemOperationId.Add(item.OperationId);
            }
            Debug.Log("ToMarket request completed. Check complete operation for getting operation status");
        }

        [ContextMenu("FromMarket item")]
        public void FromMarketRequest()
        {
            var targetItems = GetTargetItems();
            var assetIds = new string[targetItems.Length];
            for (var i = 0; i < targetItems.Length; i++)
            {
                assetIds[i] = targetItems[i].assetId;
            }
            _serverApi.AsyncFromMarket(_marketAccessToken, assetIds, FromMarketCallback, OnErrorCallback);
            Debug.Log("FromMarket request was sent.");
        }

        private void FromMarketCallback(AsyncMarketResponse result, AsyncFromMarketRequest.RequestParams request)
        {
            foreach (var item in result.Items)
            {
                _itemOperationId.Add(item.OperationId);
            }
            Debug.Log("FromMarket request completed. Check complete operation for getting operation status");
        }

        [ContextMenu("Check operation")]
        public void CheckMarketOperation()
        {
            if (_itemOperationId.Count == 0)
            {
                Debug.Log("No operation. Use To/From market to create new");
                return;
            }

            _serverApi.CheckAsyncOperation(_marketAccessToken, _itemOperationId.ToArray(),
                OnAsyncOperationCheck, OnErrorCallback);
            Debug.Log("FromMarket request was sent.");
        }

        private void OnAsyncOperationCheck(AsyncOperationRequest.Response result, AsyncOperationRequest.RequestParams request)
        {
            var logMessage = "Async operation check result: ";
            foreach (var item in result.Items)
            {
                logMessage += string.Format("\n AssetID: {0};   Status: {1}", item.assetId, item.status);
            }
            Debug.Log(logMessage);
        }

#endregion

        #region DMarket full flow

        /// <summary>
        /// Starts full DMarket flow.
        /// Flow steps:
        /// - GetBasicAccessToken.
        /// - GetMarketAccessToken using UserEmail and Password.
        /// - ToMarket item with AssetId and ClassId.
        /// - GetInMarketInventory.
        /// - FromMarket item with AssetId and ClassId.
        /// - GetInMarketInventory.
        /// </summary>
        //[ContextMenu("Start DMarketApi Flow")]
        public void StartDmarketFlow()
        {
            _serverApi.GetBasicAccessToken(_gameUserId, MarketFlow_GetBasicAccessTokenCallback, OnErrorCallback);
            Debug.Log("GetBasicAccessToken request was sent.");
        }

        private void MarketFlow_GetBasicAccessTokenCallback(BasicTokenRequest.Response result,
            BasicTokenRequest.RequestParams request)
        {
            Debug.Log(string.Format(
                "GetBasicAccessToken request completed. BasicAccessToken: {0} BasicRefreshToken: {1}", result.token,
                result.refreshToken));

            _basicAccessToken = result.token;
            _basicRefreshToken = result.refreshToken;

            _clientApi.GetMarketAccessToken(_basicAccessToken, _dMarketUserEmail, _dMarketUserPassword,
                MarketFlow_GetMarketAccessTokenCallback, OnErrorCallback);
        }

        private void MarketFlow_GetMarketAccessTokenCallback(TokenRequest.Response result,
            TokenRequest.RequestParams request)
        {
            Debug.Log(string.Format(
                "GetMarketAccessToken request completed. MarketAccessToken: {0} MarketRefreshToken: {1}.", result.token,
                result.refreshToken));

            _marketAccessToken = result.token;
            _marketRefreshToken = result.refreshToken;

            _clientApi.MarketToken = _marketAccessToken;

            _serverApi.GetInMarketInventory(_marketAccessToken, MarketFlow_GetInMarketInventoryBeforeToMarketCallback,
                OnErrorCallback);
        }

        private void MarketFlow_GetInMarketInventoryBeforeToMarketCallback(
            DMarketSDK.IntegrationAPI.Request.BasicIntegration.GetUserInventoryRequest.Response result,
            DMarketSDK.IntegrationAPI.Request.BasicIntegration.GetUserInventoryRequest.RequestParams request)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("GetInventory request before ToMarket completed. Total Items Count: {0}.",
                result.Items.Count));
            foreach (var item in result.Items)
            {
                builder.Append(
                    string.Format("\nInventory item assetId: {0}, classId: {1}.", item.assetId, item.classId));
            }

            Debug.Log(builder.ToString());

            Debug.Log("Auto flow completed");
        }
#endregion

        private void OnErrorCallback(Error error)
        {
            Debug.LogError(string.Format("ErrorCode: {0}\nMessage: {1}", error.ErrorCode, error.ErrorMessage));

            switch (error.ErrorCode)
            {
                case ErrorCode.DMarketTokenExpired:
                    Debug.LogError("Please refresh your MarketRefreshCode.");
                    break;
                case ErrorCode.CannotResolveDestinationHost:
                    Debug.LogError("There is no Internet connection");
                    break;
                case ErrorCode.RequestTimeout:
                    Debug.LogError("Timeout has come");
                    break;
            }
        }

        [ContextMenu("Generate random AssetId")]
        private void GenerateRandomItemAssetId()
        {
            _itemAssetId = GetRandomItemId();
        }

        private string GetRandomItemId()
        {
            const int minIdValue = 13;
            const int maxIdValue = int.MaxValue;
            return UnityEngine.Random.Range(minIdValue, maxIdValue).ToString();
        }
    }
}
#endif