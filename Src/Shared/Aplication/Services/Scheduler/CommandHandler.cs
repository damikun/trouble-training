using System;
using MediatR;
using System.Text.Json;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Services
{

    public class CommandHandlerOptions
    {
        public string assembly_name { get; set; } = "APIServer.Aplication";
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly IMediator _mediator;
        private readonly ITelemetry _telemetry;
        public readonly string assembly_name;
        public CommandHandler(
            IMediator mediator,
            ITelemetry telemetry,
            string assembly_name = "APIServer.Aplication")
        {
            this._mediator = mediator;
            this._telemetry = telemetry;
            this.assembly_name = assembly_name;
        }

        [DisplayName("{0}")]
        public async Task<Unit> ExecuteCommand(MediatorSerializedObject mediatorSerializedObject)
        {
            var type = GetType(mediatorSerializedObject);

            if (type != null)
            {

                dynamic req = DeserializeCommand(mediatorSerializedObject.Data, type);

                if (req != null)
                {

                    Activity activity = null;

                    if (req is ISharedCommandBase I_base_command)
                    {
                        activity = CreateActivity(I_base_command, ActivityKind.Consumer);

                        SetTraceCtxData_ParrentId(I_base_command);
                    }
                    else if (req is IRequest I_mediatr_command)
                    {
                        activity = CreateActivity(I_mediatr_command, ActivityKind.Consumer);
                    }
                    else
                    {
                        activity = CreateActivity(req, ActivityKind.Consumer);
                    }

                    SetTraceCtxData_ActivityId();

                    try
                    {
                        activity?.Start();

                        await _mediator.Send(req as IRequest);

                    }
                    catch (Exception ex)
                    {
                        _telemetry.SetOtelError(ex);

                        throw;
                    }
                    finally
                    {
                        activity?.Stop();

                        activity?.Dispose();
                    }
                }
            }

            return Unit.Value;
        }

        private dynamic DeserializeCommand(string data, Type type)
        {
            return JsonSerializer.Deserialize(data, type);
        }

        public Task ExecuteCommand(string reuest)
        {
            MediatorSerializedObject mediatorSerializedObject = JsonSerializer.Deserialize<MediatorSerializedObject>(reuest);

            return ExecuteCommand(mediatorSerializedObject);
        }

        private System.Type GetType(MediatorSerializedObject mediatorSerializedObject)
        {
            if (mediatorSerializedObject?.AssemblyName == null)
                return null;

            return Assembly.Load(assembly_name).GetType(mediatorSerializedObject.FullTypeName);
        }

        private Activity CreateActivity(object request, ActivityKind kind)
        {
            return _telemetry.AppSource.StartActivity(
                    String.Format(
                        "SchedulerEnqueue: Request<{0}>",
                        request.GetType().FullName),
                        kind, request is ISharedCommandBase sharedBase ? sharedBase.ActivityId : null);
        }

        private void SetTraceCtxData_ParrentId(ISharedCommandBase command)
        {
            if (command.ActivityId is not null)
            {
                Activity.Current.AddTag("Parrent Id", command.ActivityId);
            }
        }

        private void SetTraceCtxData_ActivityId()
        {
            if (Activity.Current?.Id is not null)
            {
                Activity.Current.AddTag("Activity Id", Activity.Current?.Id);
            }
        }
    }
}