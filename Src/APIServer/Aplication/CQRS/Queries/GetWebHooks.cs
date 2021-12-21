using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using FluentValidation;
using APIServer.Persistence;
using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Types.Pagination;
using AutoMapper.QueryableExtensions;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Core.Commands;
using APIServer.Aplication.Shared.Behaviours;
using APIServer.Aplication.Queries.Extensions;

namespace APIServer.Aplication.Queries
{

    /// <summary>
    /// Query current user
    /// </summary>
    public class GetWebHooks : CommandBase<GetWebHooksPayload>
    {
        public CursorPagingArguments arguments { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// GetWebHooks Validator
    /// </summary>
    public class GetWebHooksValidator : AbstractValidator<GetWebHooks>
    {
        public GetWebHooksValidator()
        {
            // Add Field validation..   
        }
    }

    /// <summary>
    /// Authorization validator
    /// </summary>
    public class GetWebHooksAuthorizationValidator : AuthorizationValidator<GetWebHooks>
    {
        public GetWebHooksAuthorizationValidator()
        {
            // Add Field authorization..
        }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// IGetWebHooksError
    /// </summary>
    public interface IGetWebHooksError { }

    /// <summary>
    /// GetWebHooksPayload
    /// </summary>
    public class GetWebHooksPayload : BasePayload<GetWebHooksPayload, IGetWebHooksError>
    {
        public Connection<GQL_WebHook> connection { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>Handler for <c>GetWebHooks</c> command </summary>
    public class GetWebHooksHandler : IRequestHandler<GetWebHooks, GetWebHooksPayload>
    {
        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// WebHook Queriable helper
        /// </summary>
        private readonly QueryableCursorPagination<GQL_WebHook> _pagination;

        /// <summary>
        /// Injected <c>IDbContextFactory<ApiDbContext></c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IMapper</c>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Main constructor
        /// </summary>
        public GetWebHooksHandler(
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser currentuser,
            IMapper mapper)
        {
            _mapper = mapper;

            _factory = factory;

            _current = currentuser;

            _pagination = QueryableCursorPagination<GQL_WebHook>.Instance;
        }

        /// <summary>
        /// Command handler for <c>GetWebHooks</c>
        /// </summary>
        public async Task<GetWebHooksPayload> Handle(
            GetWebHooks request, CancellationToken cancellationToken)
        {
            if (!_current.Exist)
            {
                return new GetWebHooksPayload()
                {
                    connection = null
                };
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            var query = dbContext.WebHooks
                .AsNoTracking()
                .ProjectTo<GQL_WebHook>(_mapper.ConfigurationProvider)
                .AsQueryable();

            int? totalCount = await query.CountAsync(cancellationToken);

            var connection = await _pagination
                .ApplyPaginationAsync(query, request.arguments, totalCount, cancellationToken)
                .ConfigureAwait(false);

            var response = GetWebHooksPayload.Success();

            response.connection = connection;

            return response;
        }
    }
}
