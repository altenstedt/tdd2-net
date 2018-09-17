namespace Commerce.Storage.Entities
{
    public class MoneyEntity
    {
        public MoneyEntity(long units, string currencyCode)
        {
            Units = units;
            CurrencyCode = currencyCode;
        }

        public long Units { get; set; }

        public string CurrencyCode { get; set; } // ISO 4217
    }
}