using AutoMapper;
using Commerce.Domain;
using Commerce.Host.DataContracts;

namespace Commerce.Host
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Configuration { get; private set; }

        public static IMapper Mapper { get; private set; }

        public static void Configure()
        {
            Configuration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<Product, ProductDataContract>();
                configuration.CreateMap<Money, MoneyDataContract>();
                configuration.CreateMap<Basket, BasketDataContract>();
            });

            Mapper = Configuration.CreateMapper();
        }
    }
}