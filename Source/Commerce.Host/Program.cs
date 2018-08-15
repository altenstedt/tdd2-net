using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Commerce.Host
{
    /// <summary>
    /// Entry point for service.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point for service
        /// </summary>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Build the web host
        /// </summary>
        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
