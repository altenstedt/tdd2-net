using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Host.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Host.Controllers
{
    [Route("baskets")]
    public class BasketsController : Controller
    {
        private readonly IBasketService basketService;

        public BasketsController(IBasketService basketService)
        {
            this.basketService = basketService;

            AutoMapperConfiguration.Configure();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var basket = await basketService.CreateBasket();

            var result = AutoMapperConfiguration.Mapper.Map<BasketDataContract>(basket);

            return CreatedAtRoute("GetById", new { basket.Id }, result);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetbyId(string id)
        {
            var basket = await basketService.GetBasket(id);

            var result = AutoMapperConfiguration.Mapper.Map<BasketDataContract>(basket);

            return Ok(result);
        }

        [HttpPost("{basketId}")]
        public async Task<IActionResult> Checkout([FromQuery] string basketId, [FromBody] MoneyDataContract total)
        {
            await basketService.Checkout(basketId, total.Units, total.CurrencyCode);

            return NoContent();
        }

        [HttpPost("{basketId}/product/{productId}")]
        public async Task<IActionResult> AddProductToBasket(string basketId, int productId)
        {
            await basketService.AddProductToBasket(basketId, productId);

            return NoContent();
        }
    }
}
