using System.Collections.Generic;
using System.Linq;

namespace Commerce.Domain
{
    public class Basket
    {
        private readonly List<Product> products = new List<Product>();

        public void Add(Product product)
        {
            products.Add(product);
        }

        public void Clear()
        {
            products.Clear();
        }

        public IEnumerable<Product> Products => products;

        public string Id { get; set; }

        public int Count => products.Count;

        public Money Total => products.Any() ? products.Sum(item => item.Cost) : Money.None;

        public Money TotalWithVat => Total * 1.25;
    }
}
