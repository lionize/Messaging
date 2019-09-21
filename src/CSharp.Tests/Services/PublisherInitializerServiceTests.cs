using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TIKSN.Lionize.Messaging.Tests;
using Xunit;
using Xunit.Abstractions;

namespace TIKSN.Lionize.Messaging.Services.Tests
{
    public class PublisherInitializerServiceTests
    {
        private readonly IPublisherInitializerService _publisherInitializerService;

        public PublisherInitializerServiceTests(ITestOutputHelper testOutputHelper)
        {
            var serviceProvider = HiHelper.CreateServiceProvider(testOutputHelper);
            _publisherInitializerService = serviceProvider.GetRequiredService<IPublisherInitializerService>();
        }

        [Fact]
        public async Task InitializeTest()
        {
            await _publisherInitializerService.InitializeAsync(default);
        }
    }
}