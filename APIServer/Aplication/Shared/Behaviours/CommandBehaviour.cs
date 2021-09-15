using System;
using MediatR;
using Serilog;
using System.Threading;
using APIServer.Domain;
using Aplication.Payload;
using System.Diagnostics;
using System.Threading.Tasks;
using Shared.Aplication.Interfaces;
using Shared.Aplication.Core.Commands;
using APIServer.Aplication.Shared.Errors;

namespace APIServer.Aplication.Shared.Behaviours {

    /// <summary>
    /// CommandBehaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class CommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUser _currentUserService;
        private readonly ILogger _logger;

        public CommandBehaviour(
            ICurrentUser currentUserService,
             ILogger logger) {
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            var activity = Sources.DemoSource.StartActivity(
                String.Format("CommandBehaviour: Request<{0}>", typeof(TRequest).FullName), ActivityKind.Server);

            if (typeof(TRequest).IsSubclassOf(typeof(CommandBase))) {

                ISharedCommandBase I_base_command = request as ISharedCommandBase;

                if (I_base_command.ActivityId == null
                    && Activity.Current != null
                    && Activity.Current.Id != null) {
                    I_base_command.ActivityId = Activity.Current.Id;
                }

                // This chane activity parrent / children relation..
                if (I_base_command.ActivityId != null
                    && Activity.Current != null
                    && Activity.Current.ParentId == null) {
                    Activity.Current.SetParentId(I_base_command.ActivityId);
                }
            }

            try {
                activity.Start();

                // Continue in pipe
                return await next();

            } catch (Exception ex) {

                ex.Data.Add("command_failed",true);

                var current = Activity.Current;
                current?.SetTag("otel.status_code", "ERROR");
                current?.SetTag("otel.status_description", ex.ToString());
                Log.Error(ex.ToString());

                // In case it is Mutation Response Payload = handled as payload error union
                if (Common.IsSubclassOfRawGeneric(typeof(BasePayload<,>), typeof(TResponse))) {
                    IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

                    payload.AddError(new InternalServerError(ex.Message));

                    return (TResponse)payload;
                } else {
                    throw ex;
                }

            } finally {
                activity.Stop();
                activity.Dispose();
            }
        }
    }
}