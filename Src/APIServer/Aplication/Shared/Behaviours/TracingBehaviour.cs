using System;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using APIServer.Domain;
using System.Diagnostics;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Core.Commands;
using SharedCore.Aplication.Payload;

namespace APIServer.Aplication.Shared.Behaviours {

    /// <summary>
    /// TracingBehaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TracingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUser _currentUserService;
        private readonly ILogger _logger;

        public TracingBehaviour(
            ICurrentUser currentUserService,
             ILogger logger) {
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            var activity = Sources.DemoSource.StartActivity(
                String.Format("TracingBehaviour: Request<{0}>", typeof(TRequest).FullName), ActivityKind.Server);

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

            activity.Start();
            try {
                // Continue in pipe
                return await next();

            } catch (Exception ex) {

                SharedCore.Aplication.Shared.Common.CheckAndSetOtelExceptionError(ex,_logger);

                // In case it is Mutation Response Payload = handled as payload error union
                if (SharedCore.Aplication.Shared.Common.IsSubclassOfRawGeneric(typeof(BasePayload<,>), typeof(TResponse))) {
                    return Common.HandleBaseCommandException<TResponse>(ex);
                } else {
                    throw;
                }
            } finally {
                activity.Stop();
                activity.Dispose();
            }
        }
    }
}