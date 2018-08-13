using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Commerce.Domain;
using Newtonsoft.Json;

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

            var content = await response.Content.ReadAsStringAsync();

            var dataContracts = JsonConvert.DeserializeObject<IEnumerable<WarehouseProductDataContract>>(content);

            var result = AutoMapperConfiguration.Mapper.Map<IEnumerable<Product>>(dataContracts);

            return result;
        }

        public async Task<Product> GetById(int id)
        {
            var response = await httpClient.GetAsync(new Uri(uri, $"{id}"));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var dataContracts = JsonConvert.DeserializeObject<WarehouseProductDataContract>(content);

            var result = AutoMapperConfiguration.Mapper.Map<Product>(dataContracts);

            return result;
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