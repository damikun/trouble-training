using APIServer.Domain;
using MediatR;
using SharedCore.Aplication.Core.Commands;
using SharedCore.Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace APIServer.Extensions {

    public class AppMediator : Mediator {

        private Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> _publishStrategy;

        public AppMediator(
            ServiceFactory serviceFactory
            ) : base(serviceFactory) {
        }
        
        #nullable enable
        public AppMediator(
            ServiceFactory serviceFactory,
            Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task>? publishStrategy
            ) : base(serviceFactory) {

            _publishStrategy = publishStrategy != null ? publishStrategy : SyncStopOnException;
        }
        #nullable disable

        public Task<TResponse> Send<TResponse>(ICommandBase<TResponse> request, CancellationToken cancellationToken = default) {
            return base.Send<TResponse>(request, cancellationToken);
        }

        #nullable enable
        public Task<object?> Send(ICommandBase request, CancellationToken cancellationToken = default) {
            return base.Send(request as object, cancellationToken);
        }
        #nullable disable

        private static async Task SyncStopOnException(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken) {
            foreach (var handler in handlers) {
                await handler(notification, cancellationToken).ConfigureAwait(false);
            }
        }

        protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken = default) {

            Activity activity = null;

            if (notification is INotificationBase) {

                INotificationBase I_base_notify = notification as INotificationBase;

                // If any activity is in context will be set as default in case value of ActivityId == null
                if (I_base_notify.ActivityId == null
                    && Activity.Current != null
                    && Activity.Current.Id != null) {
                    I_base_notify.ActivityId = Activity.Current.Id;
                }

                activity = Sources.DemoSource.StartActivity(
                    String.Format("PublishCore: Notification<{0}>", notification.GetType().FullName), ActivityKind.Producer);

                // This chane activity parrent / children relation..
                if (I_base_notify.ActivityId != null
                     && Activity.Current != null
                     && Activity.Current.ParentId == null) {
                    Activity.Current.SetParentId(I_base_notify.ActivityId);
                }

                if (Activity.Current != null
                    && Activity.Current.ParentId != null) {
                    Activity.Current.AddTag("Parrent Id", Activity.Current.ParentId);
                }

            } else {
                activity = Sources.DemoSource.StartActivity(
                    String.Format("PublishCore: Notification<{0}>", notification.GetType().FullName), ActivityKind.Producer);
            }

            try {
                Activity.Current.AddTag("Activity Id", Activity.Current.Id);

                activity?.Start();

                return _publishStrategy != null ? _publishStrategy(allHandlers, notification, cancellationToken) : base.PublishCore(allHandlers, notification, cancellationToken);

            } finally {
                activity?.Stop();
                activity?.Dispose();
            }
        }
    }
}