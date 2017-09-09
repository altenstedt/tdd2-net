using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce
{
    public class Basket
    {
        private readonly List<IItem> items = new List<IItem>();

        public void Add(IItem item)
        {
            items.Add(item);
        }

        public int Count => items.Count;

        public Money Total => items
            .Select(item => item.Cost)
            .Aggregate(Money.Zero, (left, right) => left + right);

        public Money TotalWithVat => Total * 1.25;
    }
}
