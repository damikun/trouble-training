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

namespace APIServer.Aplication.Shared.Behaviours {

    /// <summary>
    /// UnhandledExBehaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class UnhandledExBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUser _currentUserService;
        private readonly ILogger _logger;

        public UnhandledExBehaviour(
            ICurrentUser currentUserService,
            ILogger logger) {
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            var activity = Sources.DemoSource.StartActivity(
                String.Format("UnhandledExBehaviour: Request<{0}>", typeof(TRequest).FullName), ActivityKind.Server);

            try {
                activity.Start();

                // Continue in pipe
                return await next();

            } catch (Exception ex) {

                ex.Data.Add("command_failed",true);
                
                Common.SetOtelError(ex?.ToString(),_logger);

                // In case it is Mutation Response Payload = handled as payload error union
                if (Common.IsSubclassOfRawGeneric(typeof(BasePayload<,>), typeof(TResponse))) {
                    return Common.HandleBaseCommandException<TResponse>(ex);
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