using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Commerce.Domain;

namespace Commerce.Application
{
    public class ProductService : IProductService
    {
        // Trailing slash is needed for URI combine later
        private readonly Uri uri = new Uri("https://warehouse.azurewebsites.net/products/");

        private readonly HttpClient httpClient = new HttpClient();

        public ProductService()
        {
            AutoMapperConfiguration.Configure();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var dataContracts = await response.Content.ReadAsAsync<IEnumerable<WarehouseProductDataContract>>();

            var result = AutoMapperConfiguration.Mapper.Map<IEnumerable<Product>>(dataContracts);

            return result;
        }

        public async Task<Product> GetById(int id)
        {
            var response = await httpClient.GetAsync(new Uri(uri, $"{id}"));
            response.EnsureSuccessStatusCode();

            var dataContracts = await response.Content.ReadAsAsync<WarehouseProductDataContract>();

            var result = AutoMapperConfiguration.Mapper.Map<Product>(dataContracts);

            return result;
        }

        public async Task<bool> Exists(int id)
        {
            var message = new HttpRequestMessage(HttpMethod.Head, new Uri(uri, $"{id}"));

            var response = await httpClient.SendAsync(message);

            // If the item does not exist, the endpoint is expected to return a 404.
            // Redirects (30x) is not supported.
            return response.IsSuccessStatusCode;
        }

        internal class WarehouseProductDataContract
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public WarehouseMoneyDataContract Cost { get; set; }
        }

        internal class WarehouseMoneyDataContract
        {
            public long Units { get; set; }

            public int DecimalPlaces { get; set; }

            public string CurrencyCode { get; set; }
        }
    }
}