using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using FluentValidation;
using Aplication.Payload;
using Serilog;
using FluentValidation.Results;
using System.Diagnostics;
using Shared.Aplication.Interfaces;
using APIServer.Domain;
using APIServer.Aplication.Shared.Errors;

namespace APIServer.Aplication.Shared.Behaviours {

    /// <summary>
    /// Authorization behaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUser _currentUserService;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;

        public ValidationBehaviour(
            ICurrentUser currentUserService,
            IEnumerable<IValidator<TRequest>> validators,
             ILogger logger) {
            _currentUserService = currentUserService;
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            if (_validators.Any()) {
                var activity = Sources.DemoSource.StartActivity(
                        String.Format("ValidationBehaviour: Request<{0}>", request.GetType().FullName), ActivityKind.Server);

                try {

                    activity.Start();
                    var context = new ValidationContext<TRequest>(request);

                    var failures = _validators
                        .Select(v => v.Validate(context))
                        .SelectMany(result => result.Errors)
                        .Where(f => f != null)
                        .ToList();

                    if (failures.Count != 0) {
                        return HandleValidationErrors(failures);
                    }
                } catch (Exception ex) {

                    if(!ex.Data.Contains("command_failed")){
                        
                        ex.Data.Add("command_failed",true);

                        var current = Activity.Current;
                        current?.SetTag("otel.status_code", "ERROR");
                        current?.SetTag("otel.status_description", ex.ToString());
                        Log.Error(ex.ToString());
                    }

                    // In case it is Mutation Response Payload = handled as payload error union
                    if (Common.IsSubclassOfRawGeneric(typeof(BasePayload<,>), typeof(TResponse))) {
                        IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

                        payload.AddError(new InternalServerError(ex.Message));

                        return (TResponse)payload;
                    } else {
                        throw;
                    }

                } finally {
                    activity.Stop();
                    activity.Dispose();
                }
            }

            // Continue in pipe
            return await next();
        }

        private static TResponse HandleValidationErrors(List<ValidationFailure> error_obj) {

            // In case it is Mutation Response Payload = handled as payload error union
            if (Common.IsSubclassOfRawGeneric(typeof(BasePayload<,>), typeof(TResponse))) {
                IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

                foreach (var item in error_obj) {

                    payload.AddError(new ValidationError(item.PropertyName, item.ErrorMessage));
                }

                return (TResponse)payload;
            } else {

                if (error_obj != null) {

                    var first_item = error_obj.First();
                    if (first_item != null) {
                        throw new APIServer.Aplication.Shared.Exceptions.ValidationException(string.Format("Field: {0} - {1}", first_item.PropertyName, first_item.ErrorMessage));
                    }

                }
                throw new APIServer.Aplication.Shared.Exceptions.ValidationException("Validation error appear");

            }
        }
    }
}