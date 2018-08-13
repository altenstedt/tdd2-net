using System;
using System.Collections.Generic;
using System.Globalization;

namespace Commerce.Domain
{
    /// <summary>
    /// Represents a currency of <see cref="Money"/>.
    /// </summary>
    public class CurrencyInfo : IEquatable<CurrencyInfo>
    {
        private static readonly IDictionary<string, CurrencyInfo> Registry = new Dictionary<string, CurrencyInfo>
        {
            // Arbitrary subset of ISO 4217 currencies
            { "AED", new CurrencyInfo("AED", 2, "د.إ") },  // United Arab Emirates dirham
            { "AFN", new CurrencyInfo("AFN", 2, "؋") },    // Afghan afghani
            { "DKK", new CurrencyInfo("DKK", 2, "kr.") },  // Danish krone
            { "EUR", new CurrencyInfo("EUR", 2, "€") },    // Euro
            { "GBP", new CurrencyInfo("GBP", 2, "£") },    // Pound sterling
            { "SEK", new CurrencyInfo("SEK", 2, "kr") },   // Swedish krona
            { "USD", new CurrencyInfo("USD", 2, "$") },    // United States dollar
            { "JPY", new CurrencyInfo("JPY", 0, "¥") },    // Japanese yen
            { "JOD", new CurrencyInfo("JOD", 3, "د.ا.‏") }, // Jordanian dinar
            { "VND", new CurrencyInfo("VND", 0, "₫") }     // Vietnamese dong
        };

        private CurrencyInfo(string currencyCode, int decimalPlaces, string symbol)
        {
            CurrencyCode = currencyCode;
            DecimalPlaces = decimalPlaces;
            Symbol = symbol;
        }

        /// <summary>
        /// Initializes a new instance of the Currency class.
        /// </summary>
        /// <param name="currencyCode">The ISO 4217 currency code</param>
        /// <returns></returns>
        public static CurrencyInfo Get(string currencyCode)
        {
            if (Registry.ContainsKey(currencyCode))
            {
                return Registry[currencyCode];
            }

            throw new ArgumentException($"Unsupported currency code {currencyCode}.");
        }

        /// <summary>
        /// Gets a currency object for the provided region information.
        /// </summary>
        /// <param name="regionInfo">Region information for the currency</param>
        /// <returns>A currency object for the provided region information</returns>
        public static CurrencyInfo Get(RegionInfo regionInfo) => Get(regionInfo.ISOCurrencySymbol);

        /// <summary>
        /// Gets the ISO 4217 currency code.
        /// </summary>
        public string CurrencyCode { get; } // ISO 4217

        /// <summary>
        /// Gets the number of decimal places of the currency.
        /// </summary>
        public int DecimalPlaces { get; }

        /// <summary>
        /// Gets the localized currency symbol, suitable for human, localized display.
        /// </summary>
        public string Symbol { get; }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return CurrencyCode.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals(CurrencyInfo other)
        {
            return Equals((object)other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var value = (CurrencyInfo) obj;

            return value.CurrencyCode == CurrencyCode;
        }
    }
}