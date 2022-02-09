using MediatR;
using SharedCore.Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SharedCore.Aplication.Services
{

    public class AppMediator : Mediator
    {

        private Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task> _publishStrategy;

        internal ServiceFactory _serviceFactory;

        internal ITelemetry _telemetry;

        public AppMediator(
            ServiceFactory serviceFactory
            ) : base(serviceFactory)
        {
            _serviceFactory = serviceFactory;

            _telemetry = _serviceFactory.GetInstance<ITelemetry>();
        }

#nullable enable
        public AppMediator(
            ServiceFactory serviceFactory,
            Func<IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, CancellationToken, Task>? publishStrategy
            ) : base(serviceFactory)
        {

            _publishStrategy = publishStrategy != null ? publishStrategy : SyncStopOnException;

            _serviceFactory = serviceFactory;

            _telemetry = _serviceFactory.GetInstance<ITelemetry>();
        }
#nullable disable

        public Task<TResponse> Send<TResponse>(
            ICommandBase<TResponse> request,
            CancellationToken cancellationToken = default)
        {

            return base.Send<TResponse>(request, cancellationToken);
        }

#nullable enable
        public Task<object?> Send(
            ICommandBase request,
            CancellationToken cancellationToken = default)
        {

            return base.Send(request as object, cancellationToken);
        }
#nullable disable

        private static async Task SyncStopOnException(
            IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification,
            CancellationToken cancellationToken)
        {

            foreach (var handler in handlers)
            {
                await handler(notification, cancellationToken).ConfigureAwait(false);
            }
        }

        protected override Task PublishCore(
            IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers,
            INotification notification,
            CancellationToken cancellationToken = default)
        {

            Activity activity = StartActivity(notification);

            if (notification is INotificationBase I_base_notify)
            {
                SetTrace_ActivityId(I_base_notify);

                SetTrace_ParrentId(I_base_notify);

                SetTraceDataCtx_ParrentId();
            }

            try
            {
                SetTraceDataCtx_ActivityId();

                activity?.Start();

                return _publishStrategy != null ? _publishStrategy(allHandlers, notification, cancellationToken)
                    : base.PublishCore(allHandlers, notification, cancellationToken);

            }
            finally
            {
                activity?.Stop();
                activity?.Dispose();
            }
        }

        private Activity StartActivity(INotification notification)
        {
            return _telemetry.AppSource.StartActivity(
                    String.Format(
                        "PublishCore: Notification<{0}>",
                        notification.GetType().FullName),
                        ActivityKind.Producer);
        }

        private void SetTrace_ParrentId(INotificationBase I_base_notify)
        {
            if (I_base_notify.ActivityId != null
                && Activity.Current?.ParentId == null)
            {
                Activity.Current?.SetParentId(I_base_notify.ActivityId);
            }
        }

        private void SetTrace_ActivityId(INotificationBase I_base_notify)
        {
            // If any activity is in context will be set as default in case value of ActivityId == null
            if (I_base_notify.ActivityId == null
                && Activity.Current?.Id != null)
            {
                I_base_notify.ActivityId = Activity.Current.Id;
            }
        }

        private void SetTraceDataCtx_ParrentId()
        {
            if (Activity.Current != null
                && Activity.Current.ParentId != null)
            {
                Activity.Current?.AddTag("Parrent Id", Activity.Current.ParentId);
            }
        }

        private void SetTraceDataCtx_ActivityId()
        {
            Activity.Current?.AddTag("Activity Id", Activity.Current.Id);
        }
    }
}