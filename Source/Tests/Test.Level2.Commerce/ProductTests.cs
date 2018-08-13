using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Commerce.Host.DataContracts;
using Newtonsoft.Json;
using Xunit;

namespace Test.Level2.Commerce
{
    public class ProductTests
    {
        private readonly Uri baseUri = new Uri("http://localhost:5000/");
        private readonly HttpClient client = new HttpClient();

        [Fact]
        public async Task CanGetProducts()
        {
            var response = await client.GetAsync(new Uri(baseUri, "products"));
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadAsAsync<IEnumerable<ProductDataContract>>();

            Assert.True(products.Any());
        }
    }
}
