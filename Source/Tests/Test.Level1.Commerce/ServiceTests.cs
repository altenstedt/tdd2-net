using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Commerce.Application;
using Commerce.Domain;
using Commerce.Storage;
using Commerce.Storage.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Test.Level1.Commerce
{
    [Trait("Category", "L1")]
    public class ServiceTests
    {
        private readonly BasketService basketService;
        private readonly RegionInfo SE = new RegionInfo("SE");

        public ServiceTests()
        {
            // Set the environment variable to VALUE and create a file testsettings.VALUE.json
            // with the settings that are valid for your environment.  For example:
            // 
            // export TDD2_TEST_ENVIRONMENT=qa
            // 
            // and create a file testsettings.qa.json with the secrets for your QA database.
            var environment = Environment.GetEnvironmentVariable("TDD2_TEST_ENVIRONMENT");

            // Watch our when adding support for environment variables using
            // AddEnvironmentVariables().  For example, a typical Windows client will
            // have USERNAME defined to the logged on account name, which might
            // conflict with your application settings.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"testsettings.{environment}.json", optional: true);

            var configuration = builder.Build();

            var options = new DatabaseOptions
            {
                DatabaseName = configuration["databaseName"],
                Host = configuration["host"],
                Port = int.Parse(configuration["port"]),
                Username = configuration["username"],
                Password = configuration["password"]
            };

            var repository = new Repository(options);

            var productServiceMock = new Mock<IProductService>(MockBehavior.Strict);
            productServiceMock
                .Setup(productService => productService.GetById(42))
                .ReturnsAsync(new Product(42, "Product with difficult characaters: Ţ��", new Money(13.76, SE)));

            basketService = new BasketService(productServiceMock.Object, repository);
        }

        [Fact]
        public async Task ShouldBeEmptyWhenCreated()
        {
            var basket = await basketService.CreateBasket();

            Assert.Equal(0, basket.Count);
            Assert.Empty(basket.Products);
            Assert.Equal(Money.None, basket.Total);
            Assert.Equal(Money.None, basket.TotalWithVat);
        }

        [Fact]
        public async Task ShouldAddProductToBasket()
        {
            var created = await basketService.CreateBasket();

            await basketService.AddProductToBasket(created.Id, 42);

            var basket = await basketService.GetById(created.Id);

            Assert.Equal(1, basket.Count);
            Assert.Equal(new Money(13.76, SE), basket.Total);
            Assert.Equal(new Money(13.76, SE) * 1.25, basket.TotalWithVat);
            Assert.Equal("Product with difficult characaters: Ţ��", basket.Products.Single().Name);
        }

        [Fact]
        public async Task ShouldReturnFalseWhenNotExists()
        {
            var result = await basketService.Exists("No-such-id-exists");

            Assert.False(result);
        }

        [Fact]
        public async Task ShouldReturnNullWhenNotExists()
        {
            var result = await basketService.GetById("No-such-id-exists");

            Assert.Null(result);
        }
    }
}
