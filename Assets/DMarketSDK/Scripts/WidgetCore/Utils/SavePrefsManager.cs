using UnityEngine;

namespace DMarketSDK
{
    public static class SavePrefsManager
    {
        public const string DMARKET_USER_AGREEMENTS = "DMarketUserAgreements";

        public static string GetString(string name)
        {
            return PlayerPrefs.GetString(name);
        }

        public static int GetInt(string name)
        {
            return PlayerPrefs.GetInt(name);
        }

        public static float GetFloat(string name)
        {
            return PlayerPrefs.GetFloat(name);
        }

        public static bool GetBool(string name)
        {
            return PlayerPrefs.GetInt(name) == 1;
        }

        public static void SetString(string name, string value)
        {
            PlayerPrefs.SetString(name, value);
            PlayerPrefs.Save();
        }

        public static void SetInt(string name, int value)
        {
            PlayerPrefs.SetInt(name, value);
            PlayerPrefs.Save();
        }

        public static void SetFloat(string name, float value)
        {
            PlayerPrefs.SetFloat(name, value);
            PlayerPrefs.Save();
        }

        public static void SetBool(string name, bool value)
        {
            int intValue = (value ? 1 : 0);
            PlayerPrefs.SetInt(name, intValue);
            PlayerPrefs.Save();
        }

        public static void DeleteKey(string name)
        {
            PlayerPrefs.DeleteKey(name);
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}