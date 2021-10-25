using System;
using System.Text.Json;
using APIServer.Domain;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using SharedCore.Aplication.Core.Commands;
using SharedCore.Aplication.Interfaces;

namespace MediatR {

    public class CommandsExecutorOptions {
        public string assembly_name { get; set; } = "APIServer.Aplication";
    }

    public class CommandsExecutor {
        private readonly IMediator _mediator;
        private readonly ITelemetry _telemetry;
        public readonly string assembly_name;
        public CommandsExecutor(IMediator mediator, ITelemetry telemetry) {
            this._mediator = mediator;
            this._telemetry = telemetry;
            this.assembly_name = "APIServer.Aplication";
        }

        [DisplayName("{0}")]
        public async Task<Unit> ExecuteCommand(MediatorSerializedObject mediatorSerializedObject) {
            var type = GetType(mediatorSerializedObject);

            if (type != null) {

                dynamic req = DeserializeCommand(mediatorSerializedObject.Data,type);

                if (req != null) {

                    string activity_name = null;
                    Activity activity = null;

                    if (req is ISharedCommandBase) {

                        activity_name = String.Format(
                                       "SchedulerExecutor: Request<{0}>",
                                       (req as ISharedCommandBase).GetType().FullName);

                        ISharedCommandBase I_base_command = req as ISharedCommandBase;

                        activity = Sources.DemoSource.StartActivity(
                                    activity_name,
                                    ActivityKind.Consumer,
                                    I_base_command.ActivityId);

                        if (Activity.Current != null && Activity.Current.ParentId != null) {
                            Activity.Current.AddTag("Parrent Id", I_base_command.ActivityId);
                        }

                    } else if (req is IRequest) {

                        activity_name = String.Format(
                                       "SchedulerExecutor: Request<{0}>",
                                       (req as IRequest).GetType().FullName);

                        activity = Sources.DemoSource.StartActivity(activity_name, ActivityKind.Consumer);

                    } else {
                        activity_name = String.Format(
                                        "SchedulerExecutor: Request<{0}>",
                                        (req as object).GetType().FullName);

                        activity = Sources.DemoSource.StartActivity(activity_name, ActivityKind.Consumer);
                    }

                    if (Activity.Current != null) {
                        Activity.Current.AddTag("Activity Id", Activity.Current.Id);
                    }

                    try {
                        activity?.Start();

                        await _mediator.Send(req as IRequest);
                        
                    } catch (Exception ex) {

                        _telemetry.SetOtelError(ex);

                        throw;

                    } finally {
                        activity?.Stop();

                        activity?.Dispose();
                    }
                }
            }

            return Unit.Value;
        }

        private dynamic DeserializeCommand(string data, Type type){
            return JsonSerializer.Deserialize(data, type);
        }

        public Task ExecuteCommand(string reuest) {
            MediatorSerializedObject mediatorSerializedObject = JsonSerializer.Deserialize<MediatorSerializedObject>(reuest);

            return ExecuteCommand(mediatorSerializedObject);
        }

        private System.Type GetType(MediatorSerializedObject mediatorSerializedObject) {
            if (mediatorSerializedObject?.AssemblyName == null)
                return null;

            return Assembly.Load(assembly_name).GetType(mediatorSerializedObject.FullTypeName);
        }

    }
}