using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Domain;

namespace Commerce.Application
{
    public class FixedProductService : IProductService
    {
        private readonly RegionInfo SE = new RegionInfo("SE");

        public Task<IEnumerable<Product>> GetProducts()
        {
            var result = new Product[] { new Product(42, "Apple", new Money(13.76, SE)), new Product(42, "Banana", new Money(44.55, SE)) };

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<Product> GetById(int id)
        {
            var result = new Product(42, "Apple", new Money(13.76, SE));

            return Task.FromResult(result);
        }

        public Task<bool> Exists(int id)
        {
            return Task.FromResult(id == 42);
        }
    }
}