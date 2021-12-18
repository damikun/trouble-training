using System;
using MediatR;
using Serilog;
using System.Linq;
using System.Threading;
using FluentValidation;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Collections.Generic;
using SharedCore.Aplication.Payload;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;

namespace APIServer.Aplication.Shared.Behaviours
{

    /// <summary>
    /// Validation behaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehaviour<TRequest, TResponse>
     : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUser _currentUserService;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;
        private readonly ITelemetry _telemetry;

        public ValidationBehaviour(
            ICurrentUser currentUserService,
            IEnumerable<IValidator<TRequest>> validators,
            ILogger logger,
            ITelemetry telemetry)
        {
            _currentUserService = currentUserService;
            _validators = validators;
            _logger = logger;
            _telemetry = telemetry;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {


            if (_validators.Any())
            {

                var activity = _telemetry.AppSource.StartActivity(
                    String.Format(
                        "ValidationBehaviour: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Server);
                try
                {

                    activity?.Start();

                    var context = new ValidationContext<TRequest>(request);

                    var validationResults = await Task.WhenAll(
                        _validators.Where(v => !(v is IAuthorizationValidator))
                        .Select(v => v.ValidateAsync(context, cancellationToken)));

                    var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                    if (failures.Count != 0)
                        return HandleValidationErrors(failures);

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

            // Continue in pipe
            return await next();
        }

        private static TResponse HandleValidationErrors(List<ValidationFailure> error_obj)
        {

            // In case it is Mutation Response Payload = handled as payload error union
            if (SharedCore.Aplication.Shared.Common.IsSubclassOfRawGeneric(
                typeof(BasePayload<,>),
                typeof(TResponse))
            )
            {
                IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

                foreach (var item in error_obj)
                {

                    payload.AddError(
                        new ValidationError(
                            item.PropertyName,
                            item.ErrorMessage)
                    );
                }

                return (TResponse)payload;
            }
            else
            {

                if (error_obj != null)
                {

                    var first_item = error_obj.First();

                    if (first_item != null)
                    {
                        throw new SharedCore.Aplication.Shared.Exceptions
                        .ValidationException(
                            string.Format(
                                "Field: {0} - {1}",
                                first_item.PropertyName,
                                first_item.ErrorMessage
                            )
                        );
                    }

                }
                throw new SharedCore.Aplication.Shared.Exceptions
                .ValidationException("Validation error appear");

            }
        }
    }
}