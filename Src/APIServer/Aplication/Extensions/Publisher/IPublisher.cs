
using System.Threading;
using System.Threading.Tasks;
using Shared.Aplication.Core.Commands;

namespace APIServer.Extensions {

    public interface IPublisher {

        public Task<TResponse> Send<TResponse>(ICommandBase<TResponse> request, CancellationToken cancellationToken = default);

        public Task<object?> Send(ICommandBase request, CancellationToken cancellationToken = default);

        public Task Publish<TNotification>(TNotification notification);

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy);

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken);

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken);
    }
}