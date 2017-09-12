using System;
using Commerce;
using Xunit;

namespace Test.Unit.Commerce
{
    public class MoneyTests
    {
        [Fact]
        public void ShouldEqualToZero() {
            Assert.Equal(new Money(0), Money.Zero);
        }

        [Fact]
        public void ShouldDisplayValueWithCurrencyPrefix() {
            Assert.Equal("SEK 11,42", new Money(11.42).ToString());
        }

        [Fact]
        public void ShouldSumTwoValues() {
            Assert.Equal(new Money(12.34), new Money(11.42) + new Money(0.92));
        }

        [Fact]
        public void ShouldSubstractTwoValues() {
            Assert.Equal(new Money(107.66), new Money(123) - new Money(15.34));
        }

        [Fact]
        public void ShouldMultiply() {
            Assert.Equal(new Money(9.79), new Money(7.83) * 1.25);
        }

        [Fact]
        public void ShouldRoundDown() {
            Assert.Equal(new Money(5.52), new Money(5.522));
        }

        [Fact]
        public void ShouldRoundUp() {
            Assert.Equal(new Money(5.53), new Money(5.526));
        }
 
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1.0, 1.0)]
        [InlineData(1.0, 1.001)]
        [InlineData(1.0, 1.0049999999)]
        public void ShouldEqual(double left, double right) {
            Assert.Equal(new Money(left), new Money(right));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1.0, 1.01)]
        [InlineData(1.0, 1.006)]
        public void ShouldNotEqual(double left, double right) {
            Assert.NotEqual(new Money(left), new Money(right));
        }
    }
}