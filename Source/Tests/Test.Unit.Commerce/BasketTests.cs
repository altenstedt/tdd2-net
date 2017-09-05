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

            Assert.Equal(0, basket.Total);
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
        public void ShouldTotalWithVat()
        {
            var basket = new Basket();

            var apple = new Apple();
            var banana = new Banana();

            basket.Add(apple);
            basket.Add(banana);

            Assert.Equal(72.88, basket.TotalWithVat);
        }
    }
}
