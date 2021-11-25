
using System.Threading;
using System.Threading.Tasks;
using SharedCore.Aplication.Services;

namespace SharedCore.Aplication.Interfaces
{

    public interface IPublisher
    {

        public Task<TResponse> Send<TResponse>(ICommandBase<TResponse> request, CancellationToken cancellationToken = default);

#nullable enable
        public Task<object?> Send(ICommandBase request, CancellationToken cancellationToken = default);
#nullable disable

        public Task Publish<TNotification>(TNotification notification);

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy);

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken);

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken);
    }
}