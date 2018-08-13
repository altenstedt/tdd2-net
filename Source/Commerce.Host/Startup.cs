using Commerce.Application;
using Commerce.Storage;
using Commerce.Storage.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Remotion.Linq.Clauses;

namespace Commerce.Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseOptions>(Configuration.GetSection("mongoDb"));

            services.AddSingleton<IBasketService, BasketService>();
            services.AddSingleton<IProductService, ProductService>();

            services.AddSingleton(provider => provider.GetRequiredService<IOptions<DatabaseOptions>>().Value);
            services.AddSingleton<IRepository, Repository>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
