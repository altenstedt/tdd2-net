using System;
using Commerce.Storage;
using Microsoft.Extensions.Configuration;

namespace Test.Level1.Commerce
{
    public class ConfigurableDatabaseOptions
    {
        public ConfigurableDatabaseOptions()
        {
            var environment = Environment.GetEnvironmentVariable("COMMERCE_TEST_ENVIRONMENT") ?? "development";

            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", false)
                .AddJsonFile($"testsettings.{environment}.json", true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            Options = new DatabaseOptions
            {
                DatabaseName = configuration["mongoDb:databaseName"],
                Host = configuration["mongoDb:host"],
                Port = int.Parse(configuration["mongoDb:port"]),
                Username = configuration["mongoDb:username"],
                Password = configuration["mongoDb:password"]
            };
        }

        public DatabaseOptions Options { get; }
    }
}