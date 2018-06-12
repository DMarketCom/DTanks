using UnityEngine;

namespace DevInstruments.AutoFlow
{
    [CreateAssetMenu(fileName = "DevSettigsCatalog", menuName = "Create/Catalog/DevSettigs", order = 1)]
    public class AutoFlowSettings : ScriptableObject
    {
        public string LoginGame = "22222";
        public string PasswordGame = "22222";
        public string LoginMarket = "o.halinskyi+11@globalgames.net";
        public string PasswordMarket = "11";
    }
}
