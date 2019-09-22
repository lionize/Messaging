using FluentAssertions;
using Lionize.IntegrationMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;
using TIKSN.Lionize.Messaging.BackgroundServices;
using TIKSN.Lionize.Messaging.Handlers;
using Xunit;
using Xunit.Abstractions;

namespace TIKSN.Lionize.Messaging.Tests.BackgroundServices
{
    public class ConsumerBackgroundServiceTests
    {
        private readonly ConsumerBackgroundService<TaskUpserted> _consumerBackgroundService;
        private readonly TaskUpsertedTestHandler _taskUpsertedTestHandler;

        public ConsumerBackgroundServiceTests(ITestOutputHelper testOutputHelper)
        {
            _taskUpsertedTestHandler = new TaskUpsertedTestHandler();

            var serviceProvider = HiHelper.CreateServiceProvider(testOutputHelper, services =>
            {
                services.AddSingleton<IConsumerMessageHandler<TaskUpserted>>(_taskUpsertedTestHandler);
                services.AddHostedService<ConsumerBackgroundService<TaskUpserted>>();
            });

            _consumerBackgroundService = serviceProvider.GetServices<IHostedService>().OfType<ConsumerBackgroundService<TaskUpserted>>().Single();
        }

        [Fact]
        public async Task ConsumeTest()
        {
            TaskUpserted latest;
            _taskUpsertedTestHandler.ReceivedMessages.Subscribe(message =>
            {
                latest = message;
                _consumerBackgroundService.Dispose();
            });

            var startTask = Task.Run(async () =>
            {
                await _consumerBackgroundService.StartAsync(default);
                await _consumerBackgroundService.StopAsync(default);
            });

            await Task.Delay(5000);

            _consumerBackgroundService.Dispose();

            try
            {
                await startTask;
            }
            catch (TaskCanceledException) { }

            latest.Should().NotBeNull();
        }

        private class TaskUpsertedTestHandler : IConsumerMessageHandler<TaskUpserted>
        {
            private readonly Subject<TaskUpserted> _subject;

            public TaskUpsertedTestHandler()
            {
                _subject = new Subject<TaskUpserted>();
            }

            public IObservable<TaskUpserted> ReceivedMessages => _subject;

            public Task HandleAsync(TaskUpserted message, CorrelationID correlationID, CancellationToken cancellationToken)
            {
                _subject.OnNext(message);

                return Task.CompletedTask;
            }
        }
    }
}