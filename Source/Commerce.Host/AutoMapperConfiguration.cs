using AutoMapper;
using Commerce.Domain;
using Commerce.Host.DataContracts;

namespace Commerce.Host
{
    /// <summary>
    /// Contains methods and properties for configuration of automapper.
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Gets the MapperConfiguration instance
        /// </summary>
        public static MapperConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the IMapper instance
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Configure AutoMapper for this assembly.
        /// </summary>
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