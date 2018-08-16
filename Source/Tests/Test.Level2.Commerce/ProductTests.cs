using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Commerce.Host.DataContracts;
using Xunit;

namespace Test.Level2.Commerce
{
    [Trait("Category", "L2")]
    public class ProductTests
    {
        private readonly HttpClient client = new ConfigurableHttpClient();

        [Fact]
        public async Task CanGetProducts()
        {
            var response = await client.GetAsync("products");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadAsAsync<IEnumerable<ProductDataContract>>();

            Assert.True(products.Any());
        }
    }
}
