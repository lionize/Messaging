using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IPublisherService
    {
        Task ProduceAsync<TMessage>(TMessage message, CorrelationID correlationID, CancellationToken cancellationToken);
    }
}