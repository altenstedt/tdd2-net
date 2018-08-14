using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Host.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Host.Controllers
{
    /// <summary>
    /// REST API resources for basket data.
    /// </summary>
    [Route("baskets")]
    public class BasketsController : Controller
    {
        private readonly IBasketService basketService;
        private readonly IProductService productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsController"/> class. 
        /// </summary>
        /// <param name="basketService">The basket service</param>
        /// <param name="productService">The products service</param>
        public BasketsController(IBasketService basketService, IProductService productService)
        {
            this.basketService = basketService;
            this.productService = productService;

            AutoMapperConfiguration.Configure();
        }

        /// <summary>
        /// Create a new, empty basket.
        /// </summary>
        /// <response code="201">Basket created</response>
        /// <returns>204 Created</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BasketDataContract), 201)]
        public async Task<IActionResult> Create()
        {
            var basket = await basketService.CreateBasket();

            var result = AutoMapperConfiguration.Mapper.Map<BasketDataContract>(basket);

            return CreatedAtRoute("GetById", new { basket.Id }, result);
        }

        /// <summary>
        /// Get the basket represented by its identifier
        /// </summary>
        /// <param name="id">The basket identifier to find</param>
        /// <response code="200">Found basket</response>
        /// <response code="404">Basket not found</response>
        /// <returns>The basket that match the provided identifier</returns>
        [ProducesResponseType(typeof(BasketDataContract), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Checkout the basket.
        /// </summary>
        /// <param name="basketId">The basket identifier to checkout</param>
        /// <param name="total">The checkout total</param>
        /// <response code="204">Checkout complete</response>
        /// <response code="404">No basket found</response>
        /// <returns>204 No Content</returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Add a product to the basket
        /// </summary>
        /// <param name="basketId">The basket identifier to add the product to</param>
        /// <param name="productId">The product identifier to add to the basket</param>
        /// <response code="204">Product added to basket</response>
        /// <response code="404">Basket or product not found</response>
        /// <returns>204 No Content</returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
