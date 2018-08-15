using Commerce.Host;
using Xunit;

namespace Test.Level0.Commerce.Host
{
    [Trait("Category", "L0")]
    public class AutoMapperConfigurationTests
    {
        [Fact]
        public void AssertConfigurationIsValid() 
        {
            AutoMapperConfiguration.Configure();

            AutoMapperConfiguration.Configuration.AssertConfigurationIsValid();
        }
    }
}
