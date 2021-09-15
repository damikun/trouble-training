using Hangfire;
using MediatR;
using Newtonsoft.Json;
using System;

namespace MediatR {
    public class AsyncCommand {
        private readonly CommandsExecutor _commandsExecutor;

        public AsyncCommand(CommandsExecutor commandsExecutor) {
            _commandsExecutor = commandsExecutor;
        }

        public string Enqueue(IRequest request, string description = null) {
            var mediatorSerializedObject = SerializeObject(request, description);

            return BackgroundJob.Enqueue(() => _commandsExecutor.ExecuteCommand(mediatorSerializedObject));
        }

        public string Enqueue(MediatorSerializedObject mediatorSerializedObject) {
            return BackgroundJob.Enqueue(() => _commandsExecutor.ExecuteCommand(mediatorSerializedObject));
        }


        public string Enqueue(IRequest request, string parentJobId, JobContinuationOptions continuationOption, string description = null) {
            var mediatorSerializedObject = SerializeObject(request, description);

            return BackgroundJob.ContinueJobWith(parentJobId, () => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), continuationOption);
        }

        public string Enqueue(MediatorSerializedObject mediatorSerializedObject, string parentJobId, JobContinuationOptions continuationOption, string description = null) {
            return BackgroundJob.ContinueJobWith(parentJobId, () => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), continuationOption);
        }

        public void Schedule(IRequest request, DateTimeOffset scheduleAt, string description = null) {
            var mediatorSerializedObject = SerializeObject(request, description);
            BackgroundJob.Schedule(() => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), scheduleAt);
        }

        public void Schedule(MediatorSerializedObject mediatorSerializedObject, DateTimeOffset scheduleAt, string description = null) {
            BackgroundJob.Schedule(() => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), scheduleAt);
        }

        public void Schedule(IRequest request, TimeSpan delay, string description = null) {
            var mediatorSerializedObject = SerializeObject(request, description);
            var newTime = DateTime.Now + delay;

            BackgroundJob.Schedule(() => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), newTime);
        }
        public void Schedule(MediatorSerializedObject mediatorSerializedObject, TimeSpan delay, string description = null) {
            var newTime = DateTime.Now + delay;

            BackgroundJob.Schedule(() => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), newTime);
        }
        public void ScheduleRecurring(IRequest request, string name, string cronExpression, string description = null, string queue = "default") {
            var mediatorSerializedObject = SerializeObject(request, description);
            // JobStorage.Current.GetConnection().GetAllEntriesFromHash($"recurring-job:{"aaaa"}").
            RecurringJob.AddOrUpdate(name, () => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), cronExpression, TimeZoneInfo.Local, queue);
        }

        public void ScheduleRecurring(MediatorSerializedObject mediatorSerializedObject, string name, string cronExpression, string description = null, string queue = "default") {
            RecurringJob.AddOrUpdate(name, () => _commandsExecutor.ExecuteCommand(mediatorSerializedObject), cronExpression, TimeZoneInfo.Local, queue);
        }

        private MediatorSerializedObject SerializeObject(object mediatorObject, string description) {
            string fullTypeName = mediatorObject.GetType().FullName;
            string data = JsonConvert.SerializeObject(mediatorObject, new JsonSerializerSettings {
                Formatting = Formatting.None,
            });

            return new MediatorSerializedObject(fullTypeName, data, description);
        }
    }
}