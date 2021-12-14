using MediatR;
using System.Threading;
using FluentValidation;
using APIServer.Persistence;
using System.Threading.Tasks;
using System.Security.Claims;
using SharedCore.Aplication.Payload;
using Microsoft.EntityFrameworkCore;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Core.Commands;
using APIServer.Aplication.Shared.Behaviours;

namespace APIServer.Aplication.Queries
{

    /// <summary>
    /// Query current user
    /// </summary>
    public class GetCurrentUser : CommandBase<GetCurrentUserPayload> { }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// GetCurrentUser Validator
    /// </summary>
    public class GetCurrentUserValidator : AbstractValidator<GetCurrentUser>
    {
        public GetCurrentUserValidator() { }
    }

    /// <summary>
    /// Authorization validator
    /// </summary>
    public class GetCurrentUserAuthorizationValidator : AuthorizationValidator<GetCurrentUser>
    {
        public GetCurrentUserAuthorizationValidator()
        {
            // Add Field authorization cehcks..
        }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// IGetCurrentUserError
    /// </summary>
    public interface IGetCurrentUserError { }

    /// <summary>
    /// GetCurrentUserPayload
    /// </summary>
    public class GetCurrentUserPayload : BasePayload<GetCurrentUserPayload, IGetCurrentUserError>
    {
        public GQL_User user { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>Handler for <c>GetCurrentUser</c> command </summary>
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, GetCurrentUserPayload>
    {
        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// Main constructor
        /// </summary>
        public GetCurrentUserHandler(
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser currentuser)
        {
            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>GetCurrentUser</c>
        /// </summary>
        public async Task<GetCurrentUserPayload> Handle(
            GetCurrentUser request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (!_current.Exist)
            {
                // We return null in case user is not authenticated!
                return GetCurrentUserPayload.Success();
            }

            // You can extend object as needed
            var user = new GQL_User()
            {
                Guid = _current.UserId != null ? _current.UserId.Value.ToString() : "",
                Name = _current.GetClaim(ClaimTypes.Name),
                Email = _current.GetClaim(ClaimTypes.Email),
                SessionId = _current.GetClaim("sid"),
            };

            var response = GetCurrentUserPayload.Success();

            response.user = user;

            return response;
        }
    }
}
