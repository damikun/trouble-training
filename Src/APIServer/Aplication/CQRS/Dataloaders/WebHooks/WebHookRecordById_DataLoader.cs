using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using APIServer.Persistence;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;

namespace APIServer.Aplication.GraphQL.DataLoaders
{

    public class WebHookRecordByIdDataLoader : BatchDataLoader<long, GQL_WebHookRecord>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        private SemaphoreSlim _semaphoregate = new SemaphoreSlim(1);

        public WebHookRecordByIdDataLoader(
            IBatchScheduler scheduler,
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser current) : base(scheduler)
        {
            _current = current;
            _factory = factory;
        }

        protected override async Task<IReadOnlyDictionary<long, GQL_WebHookRecord>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {

            if (!_current.Exist)
            {
                return new List<GQL_WebHookRecord>().ToDictionary(e => e.ID, null);
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            await _semaphoregate.WaitAsync();

            try
            {
                return await dbContext.WebHooksHistory
                .AsNoTracking()
                .Where(s => keys.Contains(s.ID))
                .Select(e => new GQL_WebHookRecord
                {
                    ID = e.ID,
                    WebHookID = e.WebHookID,
                    WebHookSystemID = e.WebHookID,
                    StatusCode = e.StatusCode,
                    ResponseBody = e.ResponseBody,
                    RequestBody = e.RequestBody,
                    TriggerType = e.HookType,
                    Result = e.Result,
                    Guid = e.Guid,
                    RequestHeaders = e.RequestHeaders,
                    Exception = e.Exception,
                    Timestamp = e.Timestamp
                })
                .ToDictionaryAsync(t => t.ID, cancellationToken);
            }
            finally
            {
                _semaphoregate.Release();
            }
        }
    }
}