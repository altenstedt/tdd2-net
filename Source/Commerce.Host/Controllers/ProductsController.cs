using System.Collections.Generic;
using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Host.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Host.Controllers
{
    /// <summary>
    /// REST API resources for products data.
    /// </summary>
    [Route("products")]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsController"/> class. 
        /// </summary>
        /// <param name="productService">The products service</param>
        public ProductsController(IProductService productService)
        {
            this.productService = productService;

            AutoMapperConfiguration.Configure();
        }

        /// <summary>
        /// Get all the products available
        /// </summary>
        /// <response code="200">All available products</response>
        /// <returns>All available products</returns>
        [ProducesResponseType(typeof(BasketDataContract), 200)]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productService.GetProducts();

            var result = AutoMapperConfiguration.Mapper.Map<IEnumerable<ProductDataContract>>(products);

            return Ok(result);
        }
    }
}