namespace Commerce.Host.DataContracts
{
    /// <summary>
    /// Represents money, in a specified currency.
    /// </summary>
    public class MoneyDataContract
    {
        /// <summary>
        /// Gets or sets a value representing the fractional currency part.  
        /// </summary>
        /// <remarks>
        /// For example, USD 2.45 as 245, or JPY 5 as 5, or JOD 11.427 as 11427.
        /// </remarks>
        public long Units { get; set; }

        /// <summary>
        /// Gets or sets the number of decimal places of the currency.
        /// </summary>
        public int DecimalPlaces { get; set; }

        /// <summary>
        /// Gets or sets the ISO 4217 currency code.
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}