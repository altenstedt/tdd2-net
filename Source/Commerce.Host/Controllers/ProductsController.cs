using System.Collections.Generic;
using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Host.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace Commerce.Host.Controllers
{
    [Route("products")]
    public class ProductsController : Controller
    {
        private readonly IProductService service;

        public ProductsController(IProductService service)
        {
            this.service = service;

            AutoMapperConfiguration.Configure();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await service.GetProducts();

            var result = AutoMapperConfiguration.Mapper.Map<IEnumerable<ProductDataContract>>(products);

            return Ok(result);
        }
    }
}