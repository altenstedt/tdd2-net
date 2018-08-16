using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Commerce.Host.DataContracts;
using Microsoft.Rest.Azure;
using Xunit;

namespace Test.Level2.Commerce
{
    [Trait("Category", "L2")]
    public class BasketTests
    {
        private readonly HttpClient client = new ConfigurableHttpClient();

        [Fact]
        public async Task CanCreateBasket()
        {
            var response = await client.PostAsync("baskets", new StringContent(string.Empty));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var getResponse = await client.GetAsync(response.Headers.Location);

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        [Fact]
        public async Task CanGetBasket()
        {
            var postResponse = await client.PostAsync("baskets", new StringContent(string.Empty));
            postResponse.EnsureSuccessStatusCode();

            var response = await client.GetAsync(postResponse.Headers.Location);
            response.EnsureSuccessStatusCode();

            var basket = await response.Content.ReadAsAsync<BasketDataContract>();

            Assert.Empty(basket.Products);
            Assert.Equal(0, basket.Total.Units);
            Assert.Null(basket.Total.CurrencyCode);
            Assert.Equal(0, basket.TotalWithVat.Units);
        }

        [Fact]
        public async Task CanAddProductToBasket()
        {
            var getProductsResponse = await client.GetAsync("products");
            getProductsResponse.EnsureSuccessStatusCode();

            var products = await getProductsResponse.Content.ReadAsAsync<IEnumerable<ProductDataContract>>();
            var product = products.First(); // First() will throw if empty (no need to assert Any())

            var postResponse = await client.PostAsync("baskets", new StringContent(string.Empty));
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadAsAsync<BasketDataContract>();

            var addProductResponse = await client.PostAsync($"baskets/{created.Id}/product/{product.Id}", new StringContent(string.Empty));
            addProductResponse.EnsureSuccessStatusCode();

            // We need to get the basket again after adding products
            var response = await client.GetAsync(postResponse.Headers.Location);
            response.EnsureSuccessStatusCode();

            var basket = await response.Content.ReadAsAsync<BasketDataContract>();

            Assert.Single(basket.Products);

            Assert.Equal(product.Cost.Units, basket.Total.Units);
            Assert.Equal(product.Cost.CurrencyCode, basket.Total.CurrencyCode);

            Assert.True(basket.TotalWithVat.Units > 0);
            Assert.True(basket.TotalWithVat.Units > product.Cost.Units);
        }

        [Fact]
        public async Task CanCheckoutBasket()
        {
            // Create your test here
            await Task.CompletedTask;
        }
    }
}
