using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce
{
    public static class EnumerableExtensions
    {
        public static Money Sum(this IEnumerable<Money> items) 
        {
            return items
                .Aggregate(Money.Zero, (sum, money) => sum + money);
        }

        public static Money Sum<T>(this IEnumerable<T> items, Func<T, Money> selector) 
        {
            return items
                .Select(item => selector(item))
                .Sum();
        }
    }
}
