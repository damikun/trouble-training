using System;
using SharedCore.Aplication.Services;
using SharedCore.Aplication.Interfaces;

namespace MediatR
{
    public static class MediatorExtension
    {

        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request">Request call expression that will be marshalled to a server.</param>
        /// <param name="description"></param>
        public static string Enqueue(
            this IMediator mediator,
            IRequest request,
            string description = null)
        {

            var appmediator = mediator as AppMediator;

            var telemetry = appmediator._serviceFactory.GetInstance<ITelemetry>();

            return new Scheduler(
                new CommandHandler(mediator, telemetry), telemetry)
                    .Enqueue(request, description);
        }

        /// <summary>
        ///  Creates a new background job based on a specified method call expression
        ///  and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request"></param>
        /// <param name="scheduleAt"></param>
        /// <param name="description"></param>
        public static void Schedule(
            this IMediator mediator,
            IRequest request,
            DateTimeOffset scheduleAt,
            string description = null)
        {

            var appmediator = mediator as AppMediator;

            var telemetry = appmediator._serviceFactory.GetInstance<ITelemetry>();

            new Scheduler(new CommandHandler(mediator, telemetry), telemetry)
            .Schedule(request, scheduleAt, description);

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
        public static void ScheduleRecurring(
            this IMediator mediator,
            IRequest request,
            string name,
            string cronExpression,
            string description = null,
            string queue = "default")
        {

            var appmediator = mediator as AppMediator;

            var telemetry = appmediator._serviceFactory.GetInstance<ITelemetry>();

            new Scheduler(new CommandHandler(mediator, telemetry), telemetry)
            .ScheduleRecurring(request, name, cronExpression, description, queue);

        }
    }
}