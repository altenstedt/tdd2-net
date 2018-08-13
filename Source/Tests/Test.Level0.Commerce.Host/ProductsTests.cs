using System.Linq;
using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Domain;
using Commerce.Host.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test.Level0.Commerce.Host
{
    public class ProductsTests
    {
        [Fact]
        public async Task GetProductsShouldReturn200Ok()
        {
            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(service => service.GetProducts()).ReturnsAsync(Enumerable.Empty<Product>());

            var controller = new ProductsController(productServiceMock.Object);

            var products = await controller.GetProducts();

            Assert.IsType<OkObjectResult>(products);

            productServiceMock.Verify(service => service.GetProducts(), Times.Once);
            productServiceMock.VerifyAll();
        }
    }
}