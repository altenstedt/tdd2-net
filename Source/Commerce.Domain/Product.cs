namespace Commerce.Domain
{
    public class Product
    {
        public Product(int id, string name, Money cost)
        {
            Id = id;
            Name = name;
            Cost = cost;
        }

        public int Id { get; }

        public string Name { get; }

        public Money Cost { get; }
    }
}