using System;
using System.Globalization;

namespace Commerce.Domain
{
    /// <summary>
    /// Represents money, in a specified currency.
    /// </summary>
    /// <remarks>
    /// Money objects in two different currencies cannot be added or substracted,
    /// and will always compare to false.
    /// 
    /// Money support a "none" value, using the <see cref="None"/> proprety.  "None"
    /// represents "no money", has no currency, and can be added and subtracted to any 
    /// other currency.
    /// 
    /// <see cref="ToString"/> will display both the amount and currency information.
    /// </remarks>
    public class Money : IEquatable<Money>
    {
        private readonly MidpointRounding rounding = MidpointRounding.ToEven;

        /// <summary>
        /// Creates and initializes a new instance of class Money, with the
        /// specified units and ISO 4217 currency code.
        /// </summary>
        /// <remarks>
        /// Parameter <see cref="Units"/> should be specified in the fractional currency part.
        /// For example, USD 2.45 as 245, or JPY 5 as 5, or JOD 11.427 as 11427.
        /// </remarks>
        /// <param name="units">The amount of money in the fractional part of the currency, for example cents for USD.</param>
        /// <param name="currencyCode">The ISO 4217 currency code of the money</param>
        public Money(long units, string currencyCode)
        {
            Units = units;

            if (string.IsNullOrEmpty(currencyCode))
            {
                // This indicates "zero" money, with null currency info
                CurrencyInfo = null;
            }
            else
            {
                CurrencyInfo = CurrencyInfo.Get(currencyCode);
            }
        }

        /// <summary>
        /// Creates and initializes a new instance of class Money, with the
        /// specified units and currency code from the region information.
        /// </summary>
        /// <remarks>
        /// Parameter <see cref="Units"/> should be specified in the fractional currency part.
        /// For example, USD 2.45 as 245, or JPY 5 as 5, or JOD 11.427 as 11427.
        /// </remarks>
        /// <param name="units">The amount of money in the fractional part of the currency, for example cents for USD.</param>
        /// <param name="regionInfo">Region information for the currency</param>
        public Money(long units, RegionInfo regionInfo) : this(units, regionInfo.ISOCurrencySymbol)
        {
        }

        /// <summary>
        /// Creates and initializes a new instance of class Money, with the
        /// specified units and ISO 4217 currency code.
        /// </summary>
        /// <remarks>
        /// The amount of money will be rounded to the number of decimal digits of the currency.
        /// If value is halfway between two numbers, the even fraction number is returned; that is, 4.555 
        /// is converted to 4.56, and 5.585 is converted to 5.58.  This is sometimes called
        /// Banker's rounding (IEEE Standard 754, section 4)
        /// </remarks>
        /// <param name="value">The amount of money, subject to rounding.</param>
        /// <param name="currencyCode">The ISO 4217 currency code of the money</param>
        public Money(double value, string currencyCode) : this(0, currencyCode)
        {
            Units = Convert.ToInt64(Math.Round(value * Math.Pow(10, CurrencyInfo.DecimalPlaces), rounding));
        }

        /// <summary>
        /// Creates and initializes a new instance of class Money, with the
        /// specified units and currency code from the region information.
        /// </summary>
        /// <remarks>
        /// The amount of money will be rounded to the number of decimal digits of the currency.
        /// If value is halfway between two numbers, the even number is returned; that is, 4.555 
        /// is converted to 4.56, and 5.585 is converted to 5.58.
        /// </remarks>
        /// <param name="value">The amount of money, subject to rounding.</param>
        /// <param name="regionInfo">Region information for the currency</param>
        public Money(double value, RegionInfo regionInfo) : this(value, regionInfo.ISOCurrencySymbol)
        {
        }

        /// <summary>
        /// Gets a value representing no money.
        /// </summary>
        /// <remarks>
        /// "No money", has no currency, and can be added and subtracted to any 
        /// other currency (as zero value).
        /// </remarks>
        public static Money None => new Money(0, default(string)); // None, Empty, Void

        /// <summary>
        /// Gets a value representing zero money in the specified region.
        /// </summary>
        public static Money Zero(RegionInfo regionInfo) => new Money(0, regionInfo);

        /// <summary>
        /// Gets a value representing the fractional currency part.  
        /// </summary>
        /// <remarks>
        /// For example, USD 2.45 as 245, or JPY 5 as 5, or JOD 11.427 as 11427.
        /// </remarks>
        public long Units { get; }

        /// <summary>
        /// Gets the number of decimal places of the currency.
        /// </summary>
        public int DecimalPlaces => CurrencyInfo?.DecimalPlaces ?? 0;

        /// <summary>
        /// Gets the ISO 4217 currency code.
        /// </summary>
        public string CurrencyCode => CurrencyInfo?.CurrencyCode;

        /// <summary>
        /// Gets the currency information for this money object.
        /// </summary>
        public CurrencyInfo CurrencyInfo { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (CurrencyInfo == null)
            {
                return "None";
            }

            var scaled = Units / Math.Pow(10, CurrencyInfo.DecimalPlaces);

            var value = Math.Round(scaled, CurrencyInfo.DecimalPlaces, rounding).ToString($"F{CurrencyInfo.DecimalPlaces}", CultureInfo.InvariantCulture);

            return $"{CurrencyInfo.CurrencyCode} {value}";
        }

        /// <inheritdoc />
        public bool Equals(Money other)
        {
            return Equals((object)other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return (Money)obj == this; // This works since we also override the == operator.
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                hash = hash * 31 + Units.GetHashCode();
                hash = hash * 31 + CurrencyInfo?.GetHashCode() ?? 0;

                return hash;
            }
        }

        public static bool operator ==(Money left, Money right)
        {
            if (left is null)
            {
                return right is null;
            }

            if (right is null)
            {
                return false;
            }

            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left.CurrencyInfo is null)
            {
                return right.CurrencyInfo is null || right.Units == 0;
            }

            if (right.CurrencyInfo is null)
            {
                return left.Units == 0;
            }

            return left.Units == right.Units 
                && left.CurrencyInfo.Equals(right.CurrencyInfo);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }

        public static Money operator +(Money left, Money right)
        {
            AssertCurrency(left, right);
            AssertRounding(left, right);

            if (left.CurrencyInfo is null && right.CurrencyInfo is null)
            {
                return None;
            }

            if (left.CurrencyInfo is null)
            {
                return new Money(right.Units, right.CurrencyInfo.CurrencyCode);
            }

            if (right.CurrencyInfo is null)
            {
                return new Money(left.Units, left.CurrencyInfo.CurrencyCode);
            }

            return new Money(left.Units + right.Units, left.CurrencyInfo.CurrencyCode);
        }

        public static Money operator -(Money left, Money right)
        {
            AssertCurrency(left, right);
            AssertRounding(left, right);

            if (left.CurrencyInfo is null && right.CurrencyInfo is null)
            {
                return None;
            }

            if (left.CurrencyInfo is null)
            {
                return new Money(-right.Units, right.CurrencyInfo.CurrencyCode);
            }

            if (right.CurrencyInfo is null)
            {
                return new Money(left.Units, left.CurrencyInfo.CurrencyCode);
            }

            return new Money(left.Units - right.Units, left.CurrencyInfo.CurrencyCode);
        }

        public static Money operator *(Money money, double value)
        {
            if (money.CurrencyInfo == null)
            {
                return Money.None;
            }

            var product = money.Units * value;

            var factor = Math.Pow(10, money.CurrencyInfo.DecimalPlaces);

            return new Money(product / factor, money.CurrencyInfo.CurrencyCode);
        }

        private static void AssertRounding(Money left, Money right)
        {
            if (left.rounding != right.rounding)
            {
                throw new ArgumentException("Operands have different rounding policies.");
            }
        }

        private static void AssertCurrency(Money left, Money right)
        {
            if (left.CurrencyInfo is null || right.CurrencyInfo is null)
            {
                return;
            }

            if (left.CurrencyInfo.CurrencyCode != right.CurrencyInfo.CurrencyCode) {
                throw new ArgumentException("Operands have different currency codes.");
            }
        }
    }
}
