using System;
using Commerce;
using Xunit;

namespace Test.Unit.Commerce
{
    public class BasketTests
    {
        [Fact]
        public void ShouldContainAnItemWhenAddingOne()
        {
            var basket = new Basket();

            var apple = new Apple();

            basket.Add(apple);

            Assert.Equal(1, basket.Count);
        }

        [Fact]
        public void ShouldTotalToZeroWhenEmpty()
        {
            var basket = new Basket();

            Assert.Equal(Money.Zero, basket.Total);
        }

        [Fact]
        public void ShouldTotalToItemForOne()
        {
            var basket = new Basket();

            var apple = new Apple();

            basket.Add(apple);

            Assert.Equal(apple.Cost, basket.Total);
        }

        [Fact]
        public void ShouldTotalForManyItems()
        {
            var basket = new Basket();

            const int Count = 100;
            for (var i = 0; i < Count; i++) 
            {
                basket.Add(new Apple());
            }

            var apple = new Apple();
            Assert.Equal(apple.Cost * Count, basket.Total);
        }

        [Fact]
        public void ShouldTotalWithVat()
        {
            var basket = new Basket();

            var apple = new Apple();
            var banana = new Banana();

            basket.Add(apple);
            basket.Add(banana);

            Assert.Equal(new Money(72.89), basket.TotalWithVat);
        }
    }
}
