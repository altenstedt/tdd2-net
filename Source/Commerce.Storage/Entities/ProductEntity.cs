namespace Commerce.Storage.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MoneyEntity Cost { get; set; }
    }
}