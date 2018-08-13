using System.Collections.Generic;

namespace Commerce.Storage.Entities
{
    public class BasketEntity
    {
        public string Id { get; set; }

        public IEnumerable<ProductEntity> Items { get; set; }
    }
}