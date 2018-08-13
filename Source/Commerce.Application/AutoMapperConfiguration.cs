using AutoMapper;
using Commerce.Domain;
using Commerce.Storage.Entities;
using System.Linq;

namespace Commerce.Application
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration Configuration { get; private set; }

        public static IMapper Mapper { get; private set; }

        public static void Configure()
        {
            Configuration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<ProductService.WarehouseProductDataContract, Product>()
                    .ConstructUsing(dataContract =>
                    {
                        var cost = new Money(dataContract.Cost.Units, dataContract.Cost.CurrencyCode);

                        return new Product(dataContract.Id, dataContract.Name, cost);
                    });

                configuration.CreateMap<Money, MoneyEntity>()
                    .ConstructUsing(item => new MoneyEntity(item.Units, item.CurrencyInfo.CurrencyCode))
                    .ForMember(item => item.CurrencyCode, options => options.Ignore())
                    .ReverseMap()
                    .ConstructUsing(item => new Money(item.Units, item.CurrencyCode));

                configuration.CreateMap<Product, ProductEntity>()
                    .ForMember(entity => entity.Cost, options => options.ResolveUsing(item => new MoneyEntity(item.Cost.Units, item.Cost.CurrencyCode)))
                    .ReverseMap()
                    .ConstructUsing(entity => {
                        var cost = new Money(entity.Cost.Units, entity.Cost.CurrencyCode);
                        return new Product(entity.Id, entity.Name, cost);
                    })
                    .ForMember(entity => entity.Cost, options => options.ResolveUsing(item => new Money(item.Cost.Units, item.Cost.CurrencyCode)));

                configuration.CreateMap<Basket, BasketEntity>()
                    .ForMember(entity => entity.Items, options => options.ResolveUsing(item => item.Products))
                    .ReverseMap()
                    .ConstructUsing(entity => {
                        var model = new Basket();
                        foreach(var item in entity.Items ?? Enumerable.Empty<ProductEntity>())
                        {
                            var cost = new Money(item.Cost.Units, item.Cost.CurrencyCode);
                            var product = new Product(item.Id, item.Name, cost);

                            model.Add(product);
                        }

                        return model;
                    })
                    .ForMember(basket => basket.Products, options => options.ResolveUsing(item => item.Items))
                    .ForMember(basket => basket.Count, options => options.Ignore())
                    .ForMember(basket => basket.Total, options => options.Ignore())
                    .ForMember(basket => basket.TotalWithVat, options => options.Ignore());
            });

            Mapper = Configuration.CreateMapper();
        }
    }
}