namespace Commerce.Host.DataContracts
{
    /// <summary>
    /// Represents a product.
    /// </summary>
    public class ProductDataContract
    {
        /// <summary>
        /// Gets or sets the unique identifier for this product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this product, suitable for human display.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cost of this product.
        /// </summary>
        public MoneyDataContract Cost { get; set; }
    }
}
