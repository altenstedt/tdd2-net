using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Test.Level2.Commerce
{
    public class ConfigurableHttpClient : HttpClient
    {
        public ConfigurableHttpClient() : this(new HttpClientHandler(), true)
        {
        }

        public ConfigurableHttpClient(HttpMessageHandler handler) : this(handler, true)
        {
        }

        public ConfigurableHttpClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {
            var environment = Environment.GetEnvironmentVariable("COMMERCE_TEST_ENVIRONMENT") ?? "development";

            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", false)
                .AddJsonFile($"testsettings.{environment}.json", true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            // Scripts sometimes adds quotation marks and the end of strings
            var apiBaseUri = configuration["api:baseUri"].Trim('"');
            BaseAddress = new Uri(apiBaseUri);
        }
    }
}