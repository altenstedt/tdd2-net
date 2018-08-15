using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Domain;
using Commerce.Storage.Entities;
using Commerce.Storage.Repositories;
using Moq;
using Xunit;

namespace Test.Level0.Commerce.Application
{
    [Trait("Category", "L0")]
    public class ServiceTests
    {
        private readonly RegionInfo SE = new RegionInfo("SE");

        [Fact]
        public async Task ShouldCreateEmptyBasket()
        {
            const string BasketId = "My basket id";
            var entity = new BasketEntity { Id = BasketId };

            var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            
            repositoryMock
                .Setup(repository => repository.InsertOrUpdate(It.IsAny<BasketEntity>()))
                .ReturnsAsync(entity);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var service = new BasketService(productServiceMock.Object, repositoryMock.Object);

            var created = await service.CreateBasket();

            Assert.Equal(0, created.Count);
            Assert.Equal(BasketId, created.Id);

            repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task ShouldThrowForNonExistingBasket()
        {
            var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            
            repositoryMock
                .Setup(repository => repository.GetById(It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException());

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);

            var service = new BasketService(productServiceMock.Object, repositoryMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(null));

            repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task ShouldAddProductToBasket()
        {
            const string BasketId = "My basket id";
            var entity = new BasketEntity
            { 
                Id = BasketId
            };

            var product = new Product(42, "What name?", new Money(13.76, SE));

            var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            
            repositoryMock
                .Setup(repository => repository.GetById(BasketId))
                .ReturnsAsync(entity);

            repositoryMock
                .Setup(repository => repository.InsertOrUpdate(It.IsAny<BasketEntity>()))
                .ReturnsAsync(entity);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(productService => productService.GetById(42)).ReturnsAsync(product);

            var service = new BasketService(productServiceMock.Object, repositoryMock.Object);

            await service.AddProductToBasket(BasketId, 42);

            repositoryMock.VerifyAll();
            repositoryMock.Verify(repository => repository.InsertOrUpdate(
                It.Is<BasketEntity>(item => 
                       item.Id == BasketId
                    && item.Items.Single().Cost.Units == product.Cost.Units
                    && item.Items.Single().Id == 42)), Times.Once);

            productServiceMock.VerifyAll();
        }

        [Fact]
        public async Task ShouldThrowForNonExistingProduct()
        {
            var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);

            repositoryMock
                .Setup(repository => repository.GetById(It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException());

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(productService => productService.GetById(42)).ReturnsAsync(new Product(42, "My Product", Money.Zero(SE)));

            var service = new BasketService(productServiceMock.Object, repositoryMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddProductToBasket("a basket id", 42));

            repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task ShouldThrowForAddProductToNonExistingBasket()
        {
            const string BasketId = "My basket id";

            var product = new Product(42, "What name?", new Money(13.76, SE));

            var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            
            repositoryMock
                .Setup(repository => repository.GetById(BasketId))
                .ThrowsAsync(new InvalidOperationException());

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(productService => productService.GetById(42)).ReturnsAsync(product);

            var service = new BasketService(productServiceMock.Object, repositoryMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddProductToBasket(BasketId, 42));

            repositoryMock.VerifyAll();
        }

        [Fact]
        public async Task ShouldThrowForAddNonExistingProductToBasket()
        {
            const string BasketId = "My basket id";

            var repositoryMock = new Mock<IRepository>(MockBehavior.Strict);
            
            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock.Setup(productService => productService.GetById(42)).ThrowsAsync(new InvalidOperationException());

            var service = new BasketService(productServiceMock.Object, repositoryMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddProductToBasket(BasketId, 42));

            repositoryMock.VerifyAll();
       }
    }
}