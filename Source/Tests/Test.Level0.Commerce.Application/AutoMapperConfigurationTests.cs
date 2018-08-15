using System.Globalization;
using System.Linq;
using Commerce.Application;
using Commerce.Domain;
using Commerce.Storage.Entities;
using Xunit;

namespace Test.Level0.Commerce.Application
{
    public class AutoMapperConfigurationTests
    {
        private readonly RegionInfo SE = new RegionInfo("SE");

        [Fact]
        public void AssertConfigurationIsValid() 
        {
            AutoMapperConfiguration.Configure();

            AutoMapperConfiguration.Configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapEntityToModel()
        {
            AutoMapperConfiguration.Configure();

            const string Id = "My Id";

            var entity = new BasketEntity
            {
                Id = Id,
                Items = new[] { new ProductEntity { Id = 42,  Name = "Name", Cost = new MoneyEntity(4200, SE.ISOCurrencySymbol) } } 
            };

            var model = AutoMapperConfiguration.Mapper.Map<Basket>(entity);

            Assert.Equal(Id, model.Id);
            Assert.Single(model.Products);
            Assert.Equal(1, model.Count);
            Assert.Equal(new Money(42.0, SE), model.Total);
        }

        [Fact]
        public void ShouldMapModelToEntity()
        {
            AutoMapperConfiguration.Configure();

            var model = new Basket();

            model.Add(new Product(42, "Name", new Money(42.0, SE)));

            var entity = AutoMapperConfiguration.Mapper.Map<BasketEntity>(model);

            Assert.Equal(model.Id, entity.Id);
            Assert.Single(entity.Items);
            Assert.Equal(4200, entity.Items.Single().Cost.Units);
            Assert.Equal("SEK", entity.Items.Single().Cost.CurrencyCode);
        }
    }
}
