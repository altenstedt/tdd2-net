using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Domain;
using Commerce.Host.Controllers;
using Commerce.Host.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Test.Level0.Commerce.Host
{
    [Trait("Category", "L0")]
    public class BasketTests
    {
        [Fact]
        public async Task CreateShouldReturn201Created()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.CreateBasket()).ReturnsAsync(new Basket());

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.Create();

            Assert.IsType<CreatedAtRouteResult>(result);

            basketServiceMock.Verify(service => service.CreateBasket(), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task GetByIdShouldReturn200Ok()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.GetById(null)).ReturnsAsync(new Basket());

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.GetById(null);

            Assert.IsType<OkObjectResult>(result);

            productServiceMock.VerifyAll();

            basketServiceMock.Verify(service => service.GetById(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task GetByIdShouldReturn404NotFound()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.GetById(null)).ReturnsAsync((Basket) null);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.GetById(null);

            Assert.IsType<NotFoundResult>(result);

            productServiceMock.VerifyAll();

            basketServiceMock.Verify(service => service.GetById(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task AddProductToBasketShouldReturn204NoContent()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.AddProductToBasket(null, 0)).Returns(Task.CompletedTask);
            basketServiceMock.Setup(service => service.Exists(null)).ReturnsAsync(true);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(service => service.Exists(0)).ReturnsAsync(true);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.AddProductToBasket(null, 0);

            Assert.IsType<NoContentResult>(result);

            productServiceMock.Verify(service => service.Exists(0), Times.Once);
            productServiceMock.VerifyAll();

            basketServiceMock.Verify(service => service.AddProductToBasket(null, 0), Times.Once);
            basketServiceMock.Verify(service => service.Exists(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task AddProductToBasketShouldReturn404NotFoundForBasket()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.Exists(null)).ReturnsAsync(false);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.AddProductToBasket(null, 0);

            Assert.IsType<NotFoundObjectResult>(result);

            productServiceMock.VerifyAll();

            basketServiceMock.Verify(service => service.AddProductToBasket(null, 0), Times.Never);
            basketServiceMock.Verify(service => service.Exists(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task AddProductToBasketShouldReturn404NotFoundForProduct()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.Exists(null)).ReturnsAsync(true);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(service => service.Exists(0)).ReturnsAsync(false);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.AddProductToBasket(null, 0);

            Assert.IsType<NotFoundObjectResult>(result);

            productServiceMock.Verify(service => service.Exists(0), Times.Once);
            productServiceMock.VerifyAll();

            basketServiceMock.Verify(service => service.AddProductToBasket(null, 0), Times.Never);
            basketServiceMock.Verify(service => service.Exists(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CheckoutShouldReturn204NoContent()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.Checkout(null, 0, null)).Returns(Task.CompletedTask);
            basketServiceMock.Setup(service => service.Exists(null)).ReturnsAsync(true);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.Checkout(null, new MoneyDataContract());

            Assert.IsType<NoContentResult>(result);

            basketServiceMock.Verify(service => service.Checkout(null, 0, null), Times.Once);
            basketServiceMock.Verify(service => service.Exists(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CheckoutShouldReturn404NotFound()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.Exists(null)).ReturnsAsync(false);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object, productServiceMock.Object);

            var result = await controller.Checkout(null, new MoneyDataContract());

            Assert.IsType<NotFoundResult>(result);

            basketServiceMock.Verify(service => service.Exists(null), Times.Once);
            basketServiceMock.VerifyAll();
        }
    }
}