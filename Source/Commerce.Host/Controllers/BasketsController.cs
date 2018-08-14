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
        private readonly IProductService productService;

        public BasketsController(IBasketService basketService, IProductService productService)
        {
            this.basketService = basketService;
            this.productService = productService;

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
        public async Task<IActionResult> GetById(string id)
        {
            var basket = await basketService.GetById(id);

            if (basket == null)
            {
                return NotFound();
            }

            var result = AutoMapperConfiguration.Mapper.Map<BasketDataContract>(basket);

            return Ok(result);
        }

        [HttpPost("{basketId}")]
        public async Task<IActionResult> Checkout([FromQuery] string basketId, [FromBody] MoneyDataContract total)
        {
            if (!await basketService.Exists(basketId))
            {
                return NotFound();
            }

            await basketService.Checkout(basketId, total.Units, total.CurrencyCode);

            return NoContent();
        }

        [HttpPost("{basketId}/product/{productId}")]
        public async Task<IActionResult> AddProductToBasket(string basketId, int productId)
        {
            if (!await basketService.Exists(basketId))
            {
                return NotFound("Basket does not exists.");
            }

            if (!await productService.Exists(productId))
            {
                return NotFound("Product does not exists.");
            }

            await basketService.AddProductToBasket(basketId, productId);

            return NoContent();
        }
    }
}
