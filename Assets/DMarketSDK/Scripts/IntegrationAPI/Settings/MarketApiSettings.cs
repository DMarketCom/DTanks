using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DMarketSDK.IntegrationAPI.Settings
{
    [CreateAssetMenu(fileName = "MarketApiSettings", menuName = "Create/Catalog/MarketApiSettings")]
    public class MarketApiSettings : ScriptableObject, IClientApiSettings,
        IServerApiSettings
    {
        [Serializable]
        private class EnvironmentUrlInfo
        {
            public EnvironmentType Type = EnvironmentType.ProductionSandbox;
            public string Url = string.Empty;
        }

        [SerializeField]
        private string _gameToken;
        [SerializeField]
        private bool _useDebug = true;
        [SerializeField]
        private EnvironmentType _targetEnvironment = EnvironmentType.ProductionSandbox;
        [SerializeField]
        private List<EnvironmentUrlInfo> _environments;

        [NonSerialized]
        private string _targetEnvironmentUrl;

        public EnvironmentType TargetEnvironment
        {
            get { return _targetEnvironment; }
            set
            {
                _targetEnvironment = value;
                _targetEnvironmentUrl = GetEnvironmentUrlByType(_targetEnvironment);
            }
        }

        public string TargetEnvironmentUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_targetEnvironmentUrl))
                {
                    _targetEnvironmentUrl = GetEnvironmentUrlByType(TargetEnvironment);
                }

                return _targetEnvironmentUrl;
            }
            set { _targetEnvironmentUrl = value; }
        }

        public string GetEnvironmentUrlByType(EnvironmentType type)
        {
            return _environments.Find(c => c.Type == type).Url;
        }

        public List<EnvironmentType> GetEnvironmentTypes()
        {
            return _environments.Select(c => c.Type).ToList();
        }

        public string GameToken
        {
            get { return _gameToken; }
        }

        public bool UseDebug
        {
            get { return _useDebug; }
        }
    }
}