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
    public class GetWebHookRecords : CommandBase<GetWebHookRecordsPayload>
    {
        public CursorPagingArguments arguments { get; set; }

        public long hook_id { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// GetWebHookRecords Validator
    /// </summary>
    public class GetWebHookRecordsValidator : AbstractValidator<GetWebHookRecords>
    {
        public GetWebHookRecordsValidator() { }
    }

    /// <summary>
    /// Authorization validator
    /// </summary>
    public class GetWebHookRecordsAuthorizationValidator : AuthorizationValidator<GetWebHookRecords>
    {
        public GetWebHookRecordsAuthorizationValidator()
        {
            // Add Field authorization cehcks..
        }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// IGetWebHookRecordsError
    /// </summary>
    public interface IGetWebHookRecordsError { }

    /// <summary>
    /// GetWebHookRecordsPayload
    /// </summary>
    public class GetWebHookRecordsPayload : BasePayload<GetWebHookRecordsPayload, IGetWebHookRecordsError>
    {
        public Connection<GQL_WebHookRecord> connection { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>Handler for <c>GetWebHookRecords</c> command </summary>
    public class GetWebHookRecordsHandler : IRequestHandler<GetWebHookRecords, GetWebHookRecordsPayload>
    {
        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// WebHook Queriable helper
        /// </summary>
        private readonly QueryableCursorPagination<GQL_WebHookRecord> _pagination;

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
        public GetWebHookRecordsHandler(
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser currentuser,
            IMapper mapper)
        {
            _mapper = mapper;

            _factory = factory;

            _current = currentuser;

            _pagination = QueryableCursorPagination<GQL_WebHookRecord>.Instance;
        }

        /// <summary>
        /// Command handler for <c>GetWebHookRecords</c>
        /// </summary>
        public async Task<GetWebHookRecordsPayload> Handle(
            GetWebHookRecords request, CancellationToken cancellationToken)
        {
            if (!_current.Exist || request.hook_id <= 0)
            {
                return new GetWebHookRecordsPayload()
                {
                    connection = null
                };
            }
            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            var query = dbContext.WebHooksHistory
            .AsNoTracking()
            .Where(e => e.WebHookID == request.hook_id)
            .ProjectTo<GQL_WebHookRecord>(_mapper.ConfigurationProvider)
            .OrderByDescending(e => e.Timestamp);

            int? totalCount = await query.CountAsync(cancellationToken);

            var connection = await _pagination
                .ApplyPaginationAsync(query, request.arguments, totalCount, cancellationToken)
                .ConfigureAwait(false);

            var response = GetWebHookRecordsPayload.Success();

            response.connection = connection;

            return response;
        }
    }
}