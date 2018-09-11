using System;
using System.Text;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;
using UnityEngine;
using DMarketSDK.Market;
using DMarketSDK.IntegrationAPI.Settings;
using DMarketSDK.IntegrationAPI.Request;
using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using System.Collections.Generic;
using DMarketSDK.Domain;
using TMPro;
using UnityEngine.UI;

namespace DMarketSDK.Demo
{
    public sealed class TestingMarketApiController : MonoBehaviour
    {
        private string _sellOfferId;
        private readonly List<string> _itemOperationId = new List<string>();

        [Header("DMarket SDK")] [SerializeField]
        private ClientApi _clientApi;

        [SerializeField] private ServerApi _serverApi;
        [SerializeField] private MarketWidgetController _marketWidget;
        [SerializeField] private MarketApiSettings _marketApiSettings;
        [SerializeField] private TMP_InputField _priceAmount;

        [Header("DMarket User")] [SerializeField]
        private TMP_InputField _dMarketUserEmail;
        [SerializeField]
        private TMP_InputField _dMarketUserPassword;

        [Header("Your game user Id.")] [Tooltip("E.g: SuperUser")] [SerializeField]
        private TMP_InputField _gameUserId;

        [Header("MarketWidget item info.")] [Tooltip("Unique item instance Id. E.g: 1354679514")] [SerializeField]
        private TMP_InputField _itemAssetId;

        [Tooltip("Item type Id. E.g: AK-47")] [SerializeField]
        private TMP_InputField _itemClassId;

        [Header("DMarket Tokens.")] [Tooltip("You will receive tokens from DMarketApi.")] [SerializeField]
        private TMP_Text _marketAccessToken;

        [Tooltip("You will receive tokens from DMarketApi.")] [SerializeField]
        private TMP_Text _basicAccessToken;

        [Tooltip("You will receive tokens from DMarketApi.")] [SerializeField]
        private TMP_Text _basicRefreshToken;

        [Tooltip("You will receive tokens from DMarketApi.")] [SerializeField]
        private TMP_Text _marketRefreshToken;

        private IMarketWidget MarketWidget
        {
            get { return _marketWidget; }
        }

        #region MonoBehaviour methods

        private void Awake()
        {
            _serverApi.ApplyHttpProtocol(_marketApiSettings);
            _clientApi.ApplyHttpProtocol(_marketApiSettings);
            _itemAssetId.text = string.IsNullOrEmpty(_itemAssetId.text)
                ? GetRandomItemId() : _itemAssetId.text;
        }

        private void OnEnable()
        {
            MarketWidget.LoginEvent += OnWidgetLogin;
            MarketWidget.LogoutEvent += OnWidgetLogout;
            MarketWidget.ErrorEvent += OnErrorCallback;
        }

        private void OnDisable()
        {
            MarketWidget.LoginEvent -= OnWidgetLogin;
            MarketWidget.LogoutEvent -= OnWidgetLogout;
            MarketWidget.ErrorEvent -= OnErrorCallback;
        }

        #endregion

        #region DMarket access tokens
        
        public void GetBasicAccessToken()
        {
            _serverApi.GetBasicAccessToken(_gameUserId.text, GetBasicTokenCallback, OnErrorCallback);
        }

        private void GetBasicTokenCallback(BasicTokenRequest.Response response,
            BasicTokenRequest.RequestParams requestParams)
        {
            Debug.Log(string.Format(
                "GetBasicAccessToken request completed. BasicAccessToken: {0} BasicRefreshToken: {1}", response.token,
                response.refreshToken));

            _basicAccessToken.text = response.token;
            _basicRefreshToken.text = response.refreshToken;
        }
        
        public void GetMarketAccessToken()
        {
            _clientApi.GetMarketAccessToken(_basicAccessToken.text, _dMarketUserEmail.text, _dMarketUserPassword.text,
                GetMarketAccessTokenRequest, OnErrorCallback);
        }

        private void GetMarketAccessTokenRequest(TokenRequest.Response response, TokenRequest.RequestParams request)
        {
            Debug.Log(string.Format(
                "GetMarketAccessTokenRequest request completed. BasicAccessToken: {0} BasicRefreshToken: {1}",
                response.token, response.refreshToken));

            _marketAccessToken.text = response.token;
            _marketRefreshToken.text = response.refreshToken;

            _clientApi.MarketToken = _marketAccessToken.text;
        }
        
        public void RefreshMarketToken()
        {
            _serverApi.GetMarketRefreshToken(_marketRefreshToken.text, MarketRefreshTokenCallback, OnErrorCallback);
            Debug.Log("RefreshMarketToken request was sent.");
        }

        private void MarketRefreshTokenCallback(RefreshTokenRequest.Response result,
            RefreshTokenRequest.RequestParams request)
        {
            _marketAccessToken.text = result.token;
            _marketRefreshToken.text = result.refreshToken;

            _clientApi.MarketToken = _marketAccessToken.text;

            Debug.Log("RefreshMarketToken request completed.");
        }

        #endregion

        #region DMarket widget

        /// <summary>
        /// For open DMarketWidget you must receive BasicAccessToken and BasicRefreshToken
        /// from ServerApi. Use GetBasicAccessToken method for receive it.
        /// </summary>
        public void OpenWidget()
        {
            _serverApi.GetBasicAccessToken(_gameUserId.text, OpenWidget_GetBasicAccessTokenCallback, OnErrorCallback);
        }

        private void OpenWidget_GetBasicAccessTokenCallback(BasicTokenRequest.Response result,
            BasicTokenRequest.RequestParams request)
        {
            APITestGameModel model = new APITestGameModel();
            _basicAccessToken.text = result.token;
            _basicRefreshToken.text = result.refreshToken;
            MarketWidget.Init(_basicAccessToken.text, _basicRefreshToken.text, _clientApi);
            MarketWidget.Open(model);
        }

        private void OnWidgetLogin(LoginEventData data)
        {
            Debug.Log(string.Format("UserLogged.MarketAccessToken: {0}\nMarketRefreshAccessToken: {1}",
                data.MarketAccessToken, data.MarketRefreshAccessToken));

            _marketAccessToken.text = data.MarketAccessToken;
            _marketRefreshToken.text = data.MarketRefreshAccessToken;

            _clientApi.MarketToken = _marketAccessToken.text;
        }

        private void OnWidgetLogout()
        {
            Debug.Log("Widget Logout");
        }

        #endregion

        #region DMarketApi methods

        public void ToMarketItem()
        {
            _serverApi.AsyncToMarket(_marketAccessToken.text, GetTargetItems(), OnToMarketCallback, OnErrorCallback);
            Debug.Log("ToMarket request was sent.");
        }

        private void OnToMarketCallback(AsyncMarketResponse result, AsyncToMarketRequest.RequestParams request)
        {
            foreach (var item in result.Items)
            {
                _itemOperationId.Add(item.OperationId);
            }
            Debug.Log("ToMarket request completed. Check complete operation for getting operation status");
        }

        private AssetToMarketModel[] GetTargetItems()
        {
            return new[] { new AssetToMarketModel() { assetId = _itemAssetId.text, classId = _itemClassId.text } };
        }
        
        public void FromMarketRequest()
        {
            var targetItems = GetTargetItems();
            var assetIds = new string[targetItems.Length];
            for(var i = 0; i < targetItems.Length; i++)
            {
                assetIds[i] = targetItems[i].assetId;
            }
            _serverApi.AsyncFromMarket(_marketAccessToken.text, assetIds, FromMarketCallback, OnErrorCallback);
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

        public void CheckMarketOperation()
        {
            if (_itemOperationId.Count == 0)
            {
                Debug.Log("No operation. Use To/From market to create new");
                return;
            }

            _serverApi.CheckAsyncOperation(_marketAccessToken.text, _itemOperationId.ToArray(), 
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

        public void GetInGameInventory()
        {
            _serverApi.GetInMarketInventory(_marketAccessToken.text, GetMarketInventoryCallback, OnErrorCallback);
            Debug.Log("GetInGameInventory request was sent.");
        }

        private void GetMarketInventoryCallback(
            DMarketSDK.IntegrationAPI.Request.BasicIntegration.GetUserInventoryRequest.Response result,
            DMarketSDK.IntegrationAPI.Request.BasicIntegration.GetUserInventoryRequest.RequestParams request)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("GetInventory request completed. Total Items Count: {0}", result.Items.Count));
            foreach (var item in result.Items)
            {
                builder.Append(string.Format("\nInventory item assetId: {0}, classId: {1}", item.assetId,
                    item.classId));
            }

            Debug.Log(builder.ToString());
        }

        public void CreateSellOffer()
        {
            Price price = new Price
            {
                Amount = GetEnteredPrice()
            };
            _clientApi.CreateSellOfferRequest(_itemAssetId.text, price.Amount, price.Currency, CreateSellOfferCallback,
                OnErrorCallback);
            Debug.Log("CreateSellOffer request was sent.");
        }

        private void CreateSellOfferCallback(UserSellOfferRequest.Response result,
            UserSellOfferRequest.RequestParams request)
        {
            _sellOfferId = result.offerId;
            Debug.Log("CreateSellOffer request completed.");
        }

        public void EditSellOffer()
        {
            Price price = new Price
            {
                Amount = GetEnteredPrice()
            };
            _clientApi.EditSellOfferRequest(_sellOfferId, price.Amount, price.Currency, EditSellOfferCallback,
                OnErrorCallback);
            Debug.Log("EditSellOffer request was sent.");
        }

        private long GetEnteredPrice()
        {
            long result;
            try
            {
                result = long.Parse(_priceAmount.text);

            }
            catch (Exception e)
            {
                Debug.Log("Cannot parse price " + e.Message);
                var minPrice = 1;
                result = minPrice;
            }
            return result;
        }

        private void EditSellOfferCallback(PutUserSellOfferRequest.Response result,
            PutUserSellOfferRequest.RequestParams request)
        {
            Debug.Log("EditSellOffer request was sent.");
        }

        public void CancelSellOffer()
        {
            _clientApi.CancelSellOfferRequest(_sellOfferId, CancelSellOfferCallback, OnErrorCallback);
            Debug.Log("CancelSellOffer request was sent.");
        }

        private void CancelSellOfferCallback(UserSellOfferCancelRequest.Response result,
            UserSellOfferCancelRequest.RequestParams request)
        {
            Debug.Log("CancelSellOffer request was sent.");
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

        private string GetRandomItemId()
        {
            const int minIdValue = 13;
            const int maxIdValue = int.MaxValue;
            return UnityEngine.Random.Range(minIdValue, maxIdValue).ToString();
        }
    }
}