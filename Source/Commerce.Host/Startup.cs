using System;
using System.IO;
using Commerce.Application;
using Commerce.Storage;
using Commerce.Storage.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Commerce.Host
{
    /// <summary>
    /// Configuration and setup for starting the service.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets the configuration for the service.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Add and configure services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseOptions>(Configuration.GetSection("mongoDb"));

            services.Configure<ProductServiceOptions>(Configuration.GetSection("productService"));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<ProductServiceOptions>>().Value);

            var serviceProvider = services.BuildServiceProvider();
            var productServiceOptions = serviceProvider.GetService<ProductServiceOptions>();

            services.AddSingleton(GetProductService(productServiceOptions));
            services.AddSingleton<IBasketService, BasketService>();

            services.AddSingleton(provider => provider.GetRequiredService<IOptions<DatabaseOptions>>().Value);
            services.AddSingleton<IRepository, Repository>();

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Commerce API", Version = "v1" });

                // Enable the Swagger UI at https://host/swagger/index.html
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "Commerce.Host.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Configuration of the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Commerce v1");
            });
        }

        private IProductService GetProductService(ProductServiceOptions options)
        {
            switch (options.Service.ToLowerInvariant())
            {
                case "memory":
                    return new InMemoryProductService();

                case "production":
                    return new ProductService();

                default:
                    throw new ArgumentException(nameof(options));
            }
        }
    }
}
