using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using TIKSN.DependencyInjection;
using TIKSN.Lionize.Messaging.Options;
using Xunit.Abstractions;

namespace TIKSN.Lionize.Messaging.Tests
{
    public static class HiHelper
    {
        public static IServiceProvider CreateServiceProvider(ITestOutputHelper testOutputHelper)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddUserSecrets("TIKSN.Lionize.Messaging-ff8437fb-8fd1-4f20-8954-4de742dc60d2")
                .Build();

            var services = new ServiceCollection();
            services.AddFrameworkPlatform();
            services.AddSingleton(configurationRoot);
            services.AddLogging(config =>
            {
                var loggerConfiguration = new LoggerConfiguration();
                loggerConfiguration.WriteTo.TestOutput(testOutputHelper);
                config.AddSerilog(loggerConfiguration.CreateLogger());
            });

            services.Configure<ApplicationOptions>(options =>
            {
                options.ApplictionId = "UnitTest";
                options.ApplictionQueuePart = "unit_test";
            });

            var builder = new ContainerBuilder();
            builder.RegisterModule<MessagingAutofacModule>();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }
    }
}