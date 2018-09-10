namespace Commerce.Host
{
    /// <summary>
    /// Represents options for the product service.
    /// </summary>
    public class ProductServiceOptions
    {
        /// <summary>
        /// Gets or sets the kind of service to use.  "Memory", or "Production".
        /// </summary>
        public string Service { get; set; }
    }
}