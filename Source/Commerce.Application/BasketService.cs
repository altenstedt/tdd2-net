using System.Threading.Tasks;
using Commerce.Domain;
using Commerce.Storage.Entities;
using Commerce.Storage.Repositories;

namespace Commerce.Application
{
    public class BasketService : IBasketService
    {
        private readonly IProductService productService;
        private readonly IRepository repository;

        public BasketService(IProductService productService, IRepository repository)
        {
            this.productService = productService;
            this.repository = repository;

            AutoMapperConfiguration.Configure();
        }

        public async Task<Basket> CreateBasket()
        {
            var basket = new Basket();

            var entity = AutoMapperConfiguration.Mapper.Map<BasketEntity>(basket);

            var upserted = await repository.InsertOrUpdate(entity);

            var result = AutoMapperConfiguration.Mapper.Map<Basket>(upserted);

            return result;
        }

        public async Task<Basket> GetBasket(string id)
        {
            var entity = await repository.GetById(id);

            var model = AutoMapperConfiguration.Mapper.Map<Basket>(entity);

            return model;
        }

        public async Task AddProductToBasket(string basketId, int productId)
        {
            var product = await productService.GetById(productId);

            await AddItemToBasket(basketId, product);
        }

        public async Task Checkout(string basketId, long units, string currencyCode)
        {
            // Empty implementation, but this is where payment happens

            var basket = await GetBasket(basketId);
            basket.Clear();
        }

        private async Task AddItemToBasket(string basketId, Product product)
        {
            var basket = await GetBasket(basketId); 
            
            basket.Add(product);

            var entity = AutoMapperConfiguration.Mapper.Map<BasketEntity>(basket);

            await repository.InsertOrUpdate(entity);
        }
    }
}
