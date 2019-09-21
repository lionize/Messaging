using System.Threading;
using System.Threading.Tasks;

namespace TIKSN.Lionize.Messaging.Services
{
    public interface IPublisherInitializerService
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}