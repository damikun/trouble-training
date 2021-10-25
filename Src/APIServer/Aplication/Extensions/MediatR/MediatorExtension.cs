using System;
using APIServer.Domain;
using System.Diagnostics;
using SharedCore.Aplication.Services;
using SharedCore.Aplication.Core.Commands;

namespace MediatR {
    public static class MediatorExtension {
        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request">Request call expression that will be marshalled to a server.</param>
        /// <param name="description"></param>
        public static string Enqueue(
            this IMediator mediator,
            IRequest request,
            string description = null) {

            Activity activity = null;

            if (request is ISharedCommandBase) {
                ISharedCommandBase i_request_base = request as ISharedCommandBase;

                activity = Sources.DemoSource.StartActivity(
                    String.Format(
                        "SchedulerEnqueue: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Producer);

                i_request_base.ActivityId = Activity.Current.Id;

                // This chane activity parrent / children relation..
                if (i_request_base.ActivityId != null
                    && Activity.Current != null
                    && Activity.Current.ParentId == null) {
                    Activity.Current.SetParentId(Activity.Current.Id);
                }

                if (Activity.Current != null && Activity.Current.ParentId != null) {
                    Activity.Current.AddTag("Parrent Id", Activity.Current.ParentId);
                }

            } else {
                activity = Sources.DemoSource.StartActivity(
                    String.Format(
                        "SchedulerEnqueue: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Producer);
            }

            var telemetry = new Telemetry();

            try {

                Activity.Current.AddTag("Activity Id", Activity.Current.Id);

                activity?.Start();

                return new AsyncCommand(
                    new CommandsExecutor(mediator, telemetry)).Enqueue(request, description);

            } catch (Exception ex) {

                telemetry.SetOtelError(ex);

                throw;

            } finally {
                activity?.Stop();
                activity?.Dispose();
            }

        }

        /// <summary>
        ///  Creates a new background job based on a specified method call expression
        ///  and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request"></param>
        /// <param name="scheduleAt"></param>
        /// <param name="description"></param>
        public static void Schedule(this IMediator mediator, IRequest request, DateTimeOffset scheduleAt, string description = null) {
            new AsyncCommand(new CommandsExecutor(mediator, new Telemetry())).Schedule(request, scheduleAt, description);
        }
        /// <summary>
        /// Creates a new background job based on a specified method call expression
        /// and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <param name="cronExpression"></param>
        /// <param name="description"></param>
        /// <param name="queue"></param>
        public static void ScheduleRecurring(this IMediator mediator, IRequest request, string name, string cronExpression, string description = null, string queue = "default") {
            new AsyncCommand(new CommandsExecutor(mediator, new Telemetry())).ScheduleRecurring(request, name, cronExpression, description, queue);
        }
    }
}