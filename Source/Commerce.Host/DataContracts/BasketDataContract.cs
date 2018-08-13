using System.Collections.Generic;

namespace Commerce.Host.DataContracts
{
    /// <summary>
    /// Represents a basket of products
    /// </summary>
    public class BasketDataContract
    {
        /// <summary>
        /// Gets or sets the unique identifier for this basket.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the products that this basket contains.
        /// </summary>
        public IEnumerable<ProductDataContract> Products { get; set; }

        /// <summary>
        /// Gets or sets the total cost of this basket. 
        /// </summary>
        public MoneyDataContract Total { get; set; }

        /// <summary>
        /// Gets or sets the total cost of this basket, including VAT.
        /// </summary>
        public MoneyDataContract TotalWithVat { get; set; }
    }
}