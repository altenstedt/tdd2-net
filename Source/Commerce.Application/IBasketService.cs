using System.Threading.Tasks;
using Commerce.Domain;

namespace Commerce.Application
{
    public interface IBasketService
    {
        Task<Basket> CreateBasket();

        Task<Basket> GetById(string id);

        Task<bool> Exists(string id);

        Task AddProductToBasket(string basketId, int productId);

        Task Checkout(string basketId, long units, string currencyCode);
    }
}