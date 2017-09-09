using System;
using System.Globalization;

namespace Commerce
{
    public class Money
    {
        private readonly long units;

        private readonly int decimalPlaces = 2;

        private readonly RegionInfo regionInfo = new RegionInfo("SE"); // ISO 3166 for Sweden

        public Money(double value) 
        {
            units = Convert.ToInt64(value * Math.Pow(10, decimalPlaces));
        }

        private Money(long units) {
            this.units = units;
        }

        public static Money Zero => new Money(0);

        public override string ToString()
        {
            var scaled = units / Math.Pow(10, decimalPlaces);

            var value = Math.Round(scaled, decimalPlaces);

            return $"{regionInfo.ISOCurrencySymbol} {value:0.00}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            return obj is Money && (Money)obj == this;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                hash = hash * 31 + decimalPlaces.GetHashCode();
                hash = hash * 31 + units.GetHashCode();
                hash = hash * 31 + regionInfo.Name.GetHashCode();

                return hash;
            }
        }

        public static bool operator ==(Money left, Money right)
        {
            return left.units == right.units 
                && left.decimalPlaces == right.decimalPlaces
                && left.regionInfo.Name == right.regionInfo.Name;
        }

        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }

        public static Money operator +(Money left, Money right)
        {
            AssertRegions(left, right);

            return new Money(left.units + right.units);
        }

        public static Money operator -(Money left, Money right)
        {
            AssertRegions(left, right);

            return new Money(left.units - right.units);
        }

        public static Money operator *(Money money, double value)
        {
            var product = money.units * value;

            var factor = Math.Pow(10, money.decimalPlaces);

            return new Money(product / factor);
        }

        private static void AssertRegions(Money left, Money right)
        {
            if (left.regionInfo.Name != right.regionInfo.Name) {
                throw new ArgumentException("Operands have different regions.");
            }

            if (left.decimalPlaces != right.decimalPlaces) {
                throw new ArgumentException("Operands have different decimal places.");
            }
        }
    }
}
