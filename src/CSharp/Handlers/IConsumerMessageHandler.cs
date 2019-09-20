using System.Threading;
using System.Threading.Tasks;
using TIKSN.Integration.Correlation;

namespace TIKSN.Lionize.Messaging.Handlers
{
    public interface IConsumerMessageHandler<TMessage>
    {
        Task HandleAsync(TMessage message, CorrelationID correlationID, CancellationToken cancellationToken);
    }
}