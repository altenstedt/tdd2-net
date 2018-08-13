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
    public class BasketTests
    {
        [Fact]
        public async Task CreateShouldReturn201Cretaed()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.CreateBasket()).ReturnsAsync(new Basket());

            var controller = new BasketsController(basketServiceMock.Object);

            var result = await controller.Create();

            Assert.IsType<CreatedAtRouteResult>(result);

            basketServiceMock.Verify(service => service.CreateBasket(), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task GetByIdShouldReturn200Ok()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.GetBasket(null)).ReturnsAsync(new Basket());

            var controller = new BasketsController(basketServiceMock.Object);

            var result = await controller.GetbyId(null);

            Assert.IsType<OkObjectResult>(result);

            basketServiceMock.Verify(service => service.GetBasket(null), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task AddProductToBasketShouldReturn204NoContent()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);

            var controller = new BasketsController(basketServiceMock.Object);
            basketServiceMock.Setup(service => service.AddProductToBasket(null, 0)).Returns(Task.CompletedTask);

            var result = await controller.AddProductToBasket(null, 0);

            Assert.IsType<NoContentResult>(result);

            basketServiceMock.Verify(service => service.AddProductToBasket(null, 0), Times.Once);
            basketServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CheckoutShouldReturn204NoContent()
        {
            var basketServiceMock = new Mock<IBasketService>(MockBehavior.Strict);
            basketServiceMock.Setup(service => service.Checkout(null, 0, null)).Returns(Task.CompletedTask);

            var controller = new BasketsController(basketServiceMock.Object);

            var result = await controller.Checkout(null, new MoneyDataContract());

            Assert.IsType<NoContentResult>(result);
        }
    }
}