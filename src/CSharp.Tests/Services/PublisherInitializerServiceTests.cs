using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TIKSN.DependencyInjection;
using Xunit;

namespace TIKSN.Lionize.Messaging.Services.Tests
{
    public class PublisherInitializerServiceTests
    {
        private readonly IPublisherInitializerService _publisherInitializerService;

        public PublisherInitializerServiceTests()
        {
            var services = new ServiceCollection();
            services.AddFrameworkPlatform();
            var builder = new ContainerBuilder();
            builder.RegisterModule<MessagingAutofacModule>();
            builder.Populate(services);
            var serviceProvider = new AutofacServiceProvider(builder.Build());
            _publisherInitializerService = serviceProvider.GetRequiredService<IPublisherInitializerService>();
        }

        [Fact]
        public async Task InitializeTest()
        {
            await _publisherInitializerService.InitializeAsync(default);
        }
    }
}