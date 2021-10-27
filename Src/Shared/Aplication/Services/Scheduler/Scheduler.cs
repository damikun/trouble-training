using System;
using MediatR;
using Hangfire;
using Newtonsoft.Json;
using System.Diagnostics;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Services {

    public class Scheduler: IScheduler {

        private readonly ICommandHandler _handler;

        private readonly ITelemetry _telemetry;

        public Scheduler(
            ICommandHandler handler,
            ITelemetry telemetry) {
            _handler = handler;
            _telemetry = telemetry;
        }
        
        public string Enqueue(
            IRequest request,
            string description = null) {

            Activity activity = _telemetry.AppSource.StartActivity(
                    String.Format(
                        "SchedulerEnqueue: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Producer);;

            if (request is ISharedCommandBase) {
                ISharedCommandBase i_request_base = request as ISharedCommandBase;

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
            }

            try {

                Activity.Current?.AddTag("Activity Id", Activity.Current.Id);

                activity?.Start();

                var mediatorSerializedObject = SerializeObject(request, description);

                return BackgroundJob.Enqueue(
                    () => _handler.ExecuteCommand(mediatorSerializedObject));

            } catch (Exception ex) {

                _telemetry.SetOtelError(ex);

                throw;

            } finally {
                activity?.Stop();
                activity?.Dispose();
            }

        }

        private string Enqueue(
            IRequest request,
            string parentJobId,
            JobContinuationOptions continuationOption,
            string description = null) {

            var mediatorSerializedObject = SerializeObject(request, description);

            return BackgroundJob.ContinueJobWith(
                parentJobId,
                () => _handler.ExecuteCommand(mediatorSerializedObject), continuationOption);
        }

        private string Enqueue(
            MediatorSerializedObject mediatorSerializedObject,
            string parentJobId,
            JobContinuationOptions continuationOption,
            string description = null) {
                
            return BackgroundJob.ContinueJobWith(
                parentJobId,
                () => _handler.ExecuteCommand(mediatorSerializedObject), continuationOption);
        }

        public void Schedule(
            IRequest request,
            DateTimeOffset scheduleAt,
            string description = null) {
            
            var mediatorSerializedObject = SerializeObject(request, description);

            BackgroundJob.Schedule(
                () => _handler.ExecuteCommand(mediatorSerializedObject), scheduleAt);
        }

        public void Schedule(
            MediatorSerializedObject mediatorSerializedObject,
            DateTimeOffset scheduleAt,
            string description = null) {

            BackgroundJob.Schedule(
                () => _handler.ExecuteCommand(mediatorSerializedObject), scheduleAt);
        }

        public void Schedule(
            IRequest request,
            TimeSpan delay,
            string description = null) {
            var mediatorSerializedObject = SerializeObject(request, description);
            var newTime = DateTime.Now + delay;

            BackgroundJob.Schedule(
                () => _handler.ExecuteCommand(mediatorSerializedObject), newTime);
        }
        public void Schedule(
            MediatorSerializedObject mediatorSerializedObject,
            TimeSpan delay,
            string description = null) {

            var newTime = DateTime.Now + delay;

            BackgroundJob.Schedule(
                () => _handler.ExecuteCommand(mediatorSerializedObject), newTime);
        }
        public void ScheduleRecurring(
            IRequest request,
            string name,
            string cronExpression,
            string description = null,
            string queue = "default") {

            var mediatorSerializedObject = SerializeObject(request, description);

            RecurringJob.AddOrUpdate(
                name,
                () => _handler.ExecuteCommand(mediatorSerializedObject),
                cronExpression,
                TimeZoneInfo.Local,
                queue);
        }

        public void ScheduleRecurring(
            MediatorSerializedObject mediatorSerializedObject,
            string name,
            string cronExpression,
            string description = null,
            string queue = "default") {

            RecurringJob.AddOrUpdate(
                name,
                () => _handler.ExecuteCommand(mediatorSerializedObject),
                cronExpression,
                TimeZoneInfo.Local,
                queue);
        }

        private MediatorSerializedObject SerializeObject(
            object mediatorObject,
            string description) {

            string fullTypeName = mediatorObject.GetType().FullName;

            string data = JsonConvert.SerializeObject(mediatorObject, new JsonSerializerSettings {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.All
            });

            return new MediatorSerializedObject(fullTypeName, data, description);
        }
    }
}