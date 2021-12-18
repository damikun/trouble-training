using System;
using MediatR;
using Serilog;
using System.Linq;
using System.Threading;
using FluentValidation;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Collections.Generic;
using SharedCore.Aplication.Payload;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Shared.Attributes;
using SharedCore.Aplication.Shared.Exceptions;

namespace APIServer.Aplication.Shared.Behaviours
{

    /// <summary>
    /// Authorization marker interface for Fluent validation
    /// </summary>
    public interface IAuthorizationValidator { }

    /// <summary>
    /// Authorization validator wrapper
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class AuthorizationValidator<TRequest>
    : AbstractValidator<TRequest>, IAuthorizationValidator
    { }

    /// <summary>
    /// Authorization behaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class AuthorizationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUser _currentUserService;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;
        private readonly ITelemetry _telemetry;

        public AuthorizationBehaviour(
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

            var authorizeAttributes = request.GetType()
                .GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {

                var activity = _telemetry.AppSource.StartActivity(
                    String.Format(
                        "AuthorizationBehaviour: Request<{0}>",
                        request.GetType().FullName),
                        ActivityKind.Server);

                try
                {
                    activity?.Start();

                    // Must be authenticated user
                    if (!_currentUserService.Exist)
                        return HandleUnAuthorised(null);

                    // Role-based authorization
                    var authorizeAttributesWithRoles = authorizeAttributes.Where(
                        a => !string.IsNullOrWhiteSpace(a.Roles)
                    );

                    if (authorizeAttributesWithRoles.Any())
                    {

                        foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                        {
                            var authorized = false;

                            foreach (var role in roles)
                            {

                                if (_currentUserService.HasRole(role.Trim()))
                                {
                                    authorized = true;
                                    break;
                                }
                            }

                            // Must be a member of at least one role in roles
                            if (!authorized)
                            {
                                return HandleUnAuthorised("Role authorization failure");
                            }
                        }
                    }

                    // Policy-based authorization
                    var authorizeAttributesWithPolicies = authorizeAttributes.Where(
                        a => !string.IsNullOrWhiteSpace(a.Policy)
                    );

                    if (authorizeAttributesWithPolicies.Any())
                    {
                        foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                        {
                            if (!_currentUserService.HasRole(policy.Trim()))
                            {
                                return HandleUnAuthorised($"Policy: {policy} authorization failure");
                            }
                        }
                    }

                    // Inner command validator autorization checks
                    var authorizeAttributesWithInnerPolicies = authorizeAttributes.Where(
                        a => a.InnerPolicy == true
                    );

                    if (authorizeAttributesWithInnerPolicies.Any())
                    {

                        IValidator<TRequest>[] authValidators = _validators.Where(
                            v => v is IAuthorizationValidator).ToArray();

                        ValidationFailure[] authorization_validator_failures =
                            await CommandAuthValidationFailuresAsync(request, authValidators);

                        if (authorization_validator_failures.Any())
                            return HandleUnAuthorised(authorization_validator_failures);

                    }

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

        private static TResponse HandleUnAuthorised(object error_obj)
        {

            // In case it is Mutation Response Payload = handled as payload error union
            if (SharedCore.Aplication.Shared.Common.IsSubclassOfRawGeneric(
                typeof(BasePayload<,>),
                typeof(TResponse))
            )
            {
                IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

                if (error_obj is ValidationFailure[])
                {
                    foreach (var item in error_obj as ValidationFailure[])
                    {
                        payload.AddError(new UnAuthorised(item.CustomState, item.ErrorMessage));
                    }
                }
                else if (error_obj is string)
                {
                    payload.AddError(new UnAuthorised(error_obj as string));
                }
                else
                {
                    payload.AddError(new UnAuthorised());
                }

                return (TResponse)payload;
            }
            else
            {
                // In case it is query response = handled by global filter
                if (error_obj is ValidationFailure[])
                {
                    throw new AuthorizationException(error_obj as ValidationFailure[]);
                }
                else if (error_obj is string)
                {
                    throw new AuthorizationException(error_obj as string);
                }
                else
                {
                    throw new AuthorizationException();
                }
            }
        }

        private static async Task<ValidationFailure[]> CommandAuthValidationFailuresAsync(
            TRequest request,
            IEnumerable<IValidator<TRequest>> authValidators)
        {

            var validateTasks = authValidators
                .Select(v => v.ValidateAsync(request));

            var validateResults = await Task.WhenAll(validateTasks);

            var validationFailures = validateResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToArray();

            return validationFailures == null ?
                new ValidationFailure[0] : validationFailures;

        }
    }
}