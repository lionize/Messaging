using Lionize.IntegrationMessages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.Services;
using TIKSN.Time;
using Xunit;
using Xunit.Abstractions;

namespace TIKSN.Lionize.Messaging.Tests.Services
{
    public class PublisherServiceTests
    {
        private readonly ICorrelationService _correlationService;
        private readonly IPublisherInitializerService _publisherInitializerService;
        private readonly IPublisherService _publisherService;
        private readonly Random _random;
        private readonly ITimeProvider _timeProvider;

        public PublisherServiceTests(ITestOutputHelper testOutputHelper)
        {
            var serviceProvider = HiHelper.CreateServiceProvider(testOutputHelper);
            _publisherInitializerService = serviceProvider.GetRequiredService<IPublisherInitializerService>();
            _publisherService = serviceProvider.GetRequiredService<IPublisherService>();
            _random = serviceProvider.GetRequiredService<Random>();
            _correlationService = serviceProvider.GetRequiredService<ICorrelationService>();
            _timeProvider = serviceProvider.GetRequiredService<ITimeProvider>();
        }

        [Fact]
        public async Task PublishTest()
        {
            await _publisherInitializerService.InitializeAsync(default);

            var userId = Guid.NewGuid();

            for (int i = 0; i < 10; i++)
            {
                var buffer = new byte[16];
                _random.NextBytes(buffer);

                await _publisherService.ProduceAsync(new TaskUpserted
                {
                    Completed = _random.Next(3) == 0,
                    CreatedAt = new Moment
                    {
                        Value = _timeProvider.GetCurrentTime().ToUnixTimeMilliseconds()
                    },
                    Description = _random.Next().ToString() + _random.Next().ToString() + _random.Next().ToString(),
                    ID = new BigInteger
                    {
                        Value = buffer
                    },
                    Title = _random.Next().ToString() + _random.Next().ToString() + _random.Next().ToString(),
                    UserID = userId,
                    Subtasks = _random.Next(2) == 0 ? new List<Subtask>() : new List<Subtask>
                    {
                        new Subtask
                        {
                            Completed = _random.Next(3) == 0,
                            ID = _random.Next(),
                            Title = _random.Next().ToString() + _random.Next().ToString() + _random.Next().ToString()
                        }
                    }
                }, _correlationService.Generate(), default).ConfigureAwait(false);
            }
        }
    }
}