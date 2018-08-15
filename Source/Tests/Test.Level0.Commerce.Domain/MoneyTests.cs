using System.Globalization;
using Commerce.Domain;
using Xunit;

namespace Test.Level0.Commerce.Domain
{
    [Trait("Category", "L0")]
    public class MoneyTests
    {
        private readonly RegionInfo SE = new RegionInfo("SE");

        [Theory]
        [InlineData("SE")]
        [InlineData("JP")]
        public void ShouldEqualToZero(string regionInfoName) {
            var regionInfo = new RegionInfo(regionInfoName);
            Assert.Equal(new Money(0, regionInfo), Money.Zero(regionInfo));
        }

        [Fact]
        public void ShouldDisplayValueWithCurrencyPrefixAndDecimals() {
            Assert.Equal("SEK 11.42", new Money(11.42, "SEK").ToString());
            Assert.Equal("JPY 11", new Money(11.42, "JPY").ToString());
            Assert.Equal("JOD 11.420", new Money(11.42, "JOD").ToString());
        }

        [Theory]
        [InlineData("SE")]
        [InlineData("JP")]
        public void ShouldEqualNoneToZero(string regionInfoName)
        {
            var regionInfo = new RegionInfo(regionInfoName);
            Assert.Equal(Money.None, Money.Zero(regionInfo));
        }

        [Fact]
        public void ShouldNotEqualNoneToNonZero()
        {
            Assert.NotEqual(Money.None, new Money(1, "SEK"));
        }

        [Fact]
        public void ShouldSumNoneToOtherNone()
        {
            Assert.Equal(Money.None, Money.None + Money.None);
        }

        [Fact]
        public void ShouldSumNoneToOtherCurrency()
        {
            Assert.Equal(new Money(12.34, SE), Money.None + new Money(12.34, SE));
            Assert.Equal(Money.Zero(new RegionInfo("SE")), Money.None + Money.Zero(new RegionInfo("SE")));
        }

        [Fact]
        public void ShouldSubstractNoneToOtherNone()
        {
            Assert.Equal(Money.None, Money.None - Money.None);
        }

        [Fact]
        public void ShouldSubstractNoneToOtherCurrency()
        {
            Assert.Equal(new Money(12.34, SE), new Money(12.34, SE) - Money.None);
            Assert.Equal(Money.Zero(new RegionInfo("SE")), Money.Zero(new RegionInfo("SE")) - Money.None);
        }

        [Fact]
        public void ShouldMultiplyNoneToNone()
        {
            Assert.Equal(Money.None, Money.None * 42);
        }

        [Fact]
        public void ShouldSumTwoValues() {
            Assert.Equal(new Money(12.34, SE), new Money(11.42, SE) + new Money(0.92, SE));
        }

        [Fact]
        public void ShouldSubstractTwoValues() {
            Assert.Equal(new Money(107.66, SE), new Money(123.0, SE) - new Money(15.34, SE));
        }


        [Fact]
        public void ShouldMultiply() {
            Assert.Equal(new Money(9.79, SE), new Money(7.83, SE) * 1.25);
        }

        [Fact]
        public void ShouldRoundDown() {
            Assert.Equal(new Money(5.52, SE), new Money(5.521, SE));
            Assert.Equal(new Money(5.52, SE), new Money(5.522, SE));
            Assert.Equal(new Money(5.52, SE), new Money(5.523, SE));
            Assert.Equal(new Money(5.52, SE), new Money(5.524, SE));
            Assert.Equal(new Money(5.52, SE), new Money(5.525, SE)); // Nearest even
        }

        [Fact]
        public void ShouldRoundUp() {
            Assert.Equal(new Money(5.53, SE), new Money(5.526, SE));
            Assert.Equal(new Money(5.53, SE), new Money(5.527, SE));
            Assert.Equal(new Money(5.53, SE), new Money(5.528, SE));
            Assert.Equal(new Money(5.53, SE), new Money(5.529, SE));

            Assert.Equal(new Money(5.54, SE), new Money(5.535, SE)); // Nearest even
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 1.001)]
        [InlineData(1.0, 1.0049999999)]
        public void ShouldEqual(double left, double right) {
            Assert.Equal(new Money(left, SE), new Money(right, SE));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1.0, 1.01)]
        [InlineData(1.0, 1.006)]
        public void ShouldNotEqual(double left, double right) {
            Assert.NotEqual(new Money(left, SE), new Money(right, SE));
        }
    }
}