using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
