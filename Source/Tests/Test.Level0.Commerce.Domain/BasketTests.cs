using System.Globalization;
using Commerce.Domain;
using Xunit;

namespace Test.Level0.Commerce.Domain
{
    [Trait("Category", "L0")]
    public class BasketTests
    {
        private readonly RegionInfo SE = new RegionInfo("SE");

        [Fact]
        public void ShouldContainAnItemWhenAddingOne()
        {
            var basket = new Basket();

            var product = new Product(42, "Name", Money.Zero(SE));

            basket.Add(product);

            Assert.Equal(1, basket.Count);
        }

        [Fact]
        public void ShouldHaveNoItemsWhenCleared()
        {
            var basket = new Basket();

            var product = new Product(42, "Name", Money.Zero(SE));

            basket.Add(product);
            basket.Clear();

            Assert.Equal(0, basket.Count);
        }

        [Fact]
        public void ShouldContainMutlipleItemsWhenAddingManyOfOneProduct()
        {
            var basket = new Basket();

            var product = new Product(42, "Name", Money.Zero(SE));

            basket.Add(product);
            basket.Add(product);

            Assert.Equal(2, basket.Count);
        }

        [Fact]
        public void ShouldTotalToNoneWhenEmpty()
        {
            var basket = new Basket();

            Assert.Equal(Money.None, basket.Total);
        }

        [Fact]
        public void ShouldTotalToItemForOne()
        {
            var basket = new Basket();

            var product = new Product(42, "Name", new Money(42.0, SE));

            basket.Add(product);

            Assert.Equal(product.Cost, basket.Total);
        }

        [Fact]
        public void ShouldTotalForManyItems()
        {
            var basket = new Basket();

            var cost = new Money(12.34, SE);

            const int Count = 100;
            for (var i = 0; i < Count; i++) 
            {
                basket.Add(new Product(42, "Name", cost));
            }

            Assert.Equal(cost * Count, basket.Total);
        }

        [Fact]
        public void ShouldTotalWithVat()
        {
            var basket = new Basket();

            var apple = new Product(42, "Apple", new Money(13.76, SE));
            var banana = new Product(43, "Banana", new Money(44.55, SE));

            basket.Add(apple);
            basket.Add(banana);

            Assert.Equal(new Money(72.89, SE), basket.TotalWithVat);
        }
    }
}
