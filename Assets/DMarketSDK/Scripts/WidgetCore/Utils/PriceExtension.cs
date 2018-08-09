using DMarketSDK.IntegrationAPI.Request;
using System.Collections.Generic;
using SHLibrary.Logging;
using UnityEngine;
using System.Globalization;

namespace DMarketSDK
{
    public static class PriceExtension
    {
        //TODO wait backend changing
        private const double ShowingPriceAmountDivisor = 100000000;

        public static double MaxPriceAmount
        {
            get { return ShowingPriceAmountDivisor - 1; }
        }

        public static double MinPriceAmount
        {
            get { return 1d / ShowingPriceAmountDivisor; }
        }

        private static readonly Dictionary<string, string> BindedCurrencyNames = new Dictionary<string, string>
        {
            {Price.DMC, "dmc"}
        };

        private const string SpriteTagFormat = "<sprite name=\"{0}\" color=\"#{1}\">{2}";

        public static string GetStringWithCurrencySprite(this Price price, Color color)
        {
            var currencySpriteTag = GetCurrencySpriteName(price.Currency);
            return string.Format(SpriteTagFormat, currencySpriteTag, ColorUtility.ToHtmlStringRGBA(color), price.FormatPriceText());
        }

        public static string GetCurrencyIconString(this Price price, Color color)
        {
            var currencySpriteTag = GetCurrencySpriteName(price.Currency);
            return string.Format(SpriteTagFormat, currencySpriteTag, ColorUtility.ToHtmlStringRGBA(color), string.Empty);
        }

        public static string FormatPriceText(this Price price) // TODO: need refactor and add to format "1,000,000.00000000". Now its looks like "1000000.00000000"
        {
            double amount = (double)(price.Amount / (decimal)ShowingPriceAmountDivisor);

            return amount.ToString("N1", CultureInfo.InvariantCulture);
        }

        public static string FormatPriceText(double amount)
        {
            return amount.ToString("0.##########");
        }

        public static double ConvertAmountToDouble(long amount)
        {
            return (double) (amount / (decimal) ShowingPriceAmountDivisor);
        }

        public static long ConvertDoubleToAmount(double amount)
        {
            return (long)((decimal)amount * (decimal)ShowingPriceAmountDivisor);
        }

        private static string GetCurrencySpriteName(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return string.Empty;
            }
            if (BindedCurrencyNames.ContainsKey(currency))
            {
                return BindedCurrencyNames[currency];
            }
            DevLogger.Error(string.Format("Currency {0} not binded", currency));
            return string.Empty;
        }
    }
}